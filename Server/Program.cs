using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ScorePALServer.DAO.Implementation;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Exceptions;
using ScorePALServer.Service.Implementation;
using ScorePALServer.Service.Interfaces;
using ScorePALServer.SSE;

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
builder.Services.AddScoped<IMatchDAO, MatchDAO>();

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

// Mapping des contrôleurs REST&
app.MapControllers();
app.UseMiddleware<ErrorHandler>();

await app.RunAsync();