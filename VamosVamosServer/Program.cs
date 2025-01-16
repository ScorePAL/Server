using System.Text.Json;
using VamosVamosServer.Controllers;
using VamosVamosServer.DAO.Implementation;
using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Service.Implementation;
using VamosVamosServer.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection

builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchDAO, MatchDAO>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDAO, UserDAO>();

var app = builder.Build();

// Swagger Configuration
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.MapGet("/", async (HttpContext ctx, ItemService service, CancellationToken ct) =>
{
    ctx.Response.Headers.Add("Content-Type", "text/event-stream");

    while (!ct.IsCancellationRequested)
    {
        var item = await service.WaitForNewItem();

        await ctx.Response.WriteAsync($"data: ");
        await JsonSerializer.SerializeAsync(ctx.Response.Body, item);
        await ctx.Response.WriteAsync($"\n\n");
        await ctx.Response.Body.FlushAsync();

        service.Reset();
    }
});

// Mapping des contrôleurs REST&
app.MapControllers();

await app.RunAsync();
