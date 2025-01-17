using VamosVamosServer.DAO.Implementation;
using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Service.Implementation;
using VamosVamosServer.Service.Interfaces;
using VamosVamosServer.SSE;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<SSEController>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection

builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchDAO, MatchDAO>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDAO, UserDAO>();

builder.Services.AddScoped<ICounterService, CounterService>();

var app = builder.Build();

// Swagger Configuration
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapGet("/sse", async Task (HttpContext ctx, ICounterService service, CancellationToken token) =>
{
    var controller = ctx.RequestServices.GetRequiredService<SSEController>();
    controller.Subscribe(1, ctx, token);
    await controller.SendMessage("test", 1);
});

// Mapping des contrôleurs REST&
app.MapControllers();

await app.RunAsync();
