using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ScorePALServerModel.DAO.Implementation;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServerModel.Exceptions;
using ScorePALServerController.SSE;
using ScorePALServerService.Implementation;
using ScorePALServerService.Interfaces;
using Shared.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();

// JWT
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory()) // racine du projet
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Services.Configure<ConnectionStrings>(configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<OAuthConfig>(configuration.GetSection("OAuth"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme )
   .AddJwtBearer (o =>
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
    // Keep connection alive until client disconnects
    await Task.Delay(Timeout.Infinite, token);
});

// Rest controllers Mapping
app.MapControllers();
app.UseMiddleware<ErrorHandler>();

await app.RunAsync();