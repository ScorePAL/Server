using System.Net;
using System.Net.WebSockets;
using System.Text;
using VamosVamosServer.DAO.Implementation;
using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Service.Implementation;
using VamosVamosServer.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchDAO, MatchDAO>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        while (true)
        {
            var message = "The current time is " + DateTime.Now.ToString("HH:mm:ss");
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(bytes, 0, bytes.Length);
            if (ws.State == WebSocketState.Open)
            {
                await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            else
            {
                break;
            }

            await Task.Delay(1000);
        }
    }
    else
    {
        context.Response.StatusCode = (int) HttpStatusCode.BadRequest;
    }
});

await app.RunAsync();