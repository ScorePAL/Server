using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using ScorePALServerController.Middlewares.Logging;
using ScorePALServerModel.DAO.Implementation;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServerModel.Exceptions;
using ScorePALServerController.SSE;
using ScorePALServerService.Implementation;
using ScorePALServerService.Interfaces;
using Shared.Configuration;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()  // Add console output for Docker
    .WriteTo.File("Logs/latest-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Custom HTTP logging configuration to exclude sensitive data
builder.Services.AddHttpLogging(logging =>
{
    // Reduce logging fields to exclude request/response bodies by default
    logging.LoggingFields = HttpLoggingFields.RequestPath |
                           HttpLoggingFields.RequestMethod |
                           HttpLoggingFields.RequestScheme |
                           HttpLoggingFields.ResponseStatusCode |
                           HttpLoggingFields.RequestHeaders |
                           HttpLoggingFields.ResponseHeaders;

    // Only log specific headers (avoid Authorization header)
    logging.RequestHeaders.Add("User-Agent");
    logging.RequestHeaders.Add("Content-Type");
    logging.RequestHeaders.Add("sec-ch-ua");

    logging.ResponseHeaders.Add("Content-Type");

    // Exclude sensitive headers
    logging.RequestHeaders.Remove("Authorization");
    logging.RequestHeaders.Remove("Cookie");

    // Custom request body logging predicate
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
    logging.CombineLogs = true;
});

// JWT
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<OAuthConfig>(configuration.GetSection("OAuth"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        var value = configuration.GetSection("OAuth").GetSection("Key").Value;
        if (value != null)
            o.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(value)),
                ValidIssuer = "https://localhost/",
                ValidAudience = "https://localhost/api",
                ClockSkew = TimeSpan.Zero
            };
    });

// Dependency Injection
// Controllers, DAOs and Services
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchDao, MatchDAO>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDAO, UserDAO>();

builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITeamDAO, TeamDAO>();

builder.Services.AddScoped<ITokenService, TokenService>();

// SSE
builder.Services.AddSingleton<IConnectionManager, ConnectionManager>();
builder.Services.AddSingleton<IEventPublisher, SSEEventPublisher>();

var app = builder.Build();

// Use the custom middleware for sensitive data handling
app.UseMiddleware<SensitiveDataLoggingMiddleware>();

// HTTP logging with reduced scope
app.UseHttpLogging();

// Swagger Configuration
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapGet("/sse", async (HttpContext ctx,
    IConnectionManager connectionManager,
    CancellationToken token,
    long clubId) =>
{
    await connectionManager.AddConnection(clubId, ctx, token);
    await Task.Delay(Timeout.Infinite, token);
});

// Rest controllers Mapping
app.MapControllers();
app.UseMiddleware<ErrorHandler>();

await app.RunAsync();