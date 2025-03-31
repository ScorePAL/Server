using ScorePALServer;
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

// Dependency Injection
// Controllers, DAOs and Services
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchDAO, MatchDAO>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDAO, UserDAO>();

builder.Services.AddScoped<ICounterService, CounterService>();

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