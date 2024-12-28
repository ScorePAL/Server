using System.Net;
using System.Net.WebSockets;
using VamosVamosServer.DAO.Implementation;
using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Service.Implementation;
using VamosVamosServer.Service.Interfaces;
using WebSocketManager = VamosVamosServer.WebSockets.WebSocketManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependency Injection
builder.Services.AddSingleton<WebSocketManager>(); // Singleton pour un seul WebSocketManager global
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IMatchDAO, MatchDAO>();

var app = builder.Build();

// Swagger Configuration
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

// Middleware WebSocket
app.UseWebSockets();
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        // Récupérer le WebSocketManager injecté
        var webSocketManager = app.Services.GetRequiredService<WebSocketManager>();

        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        webSocketManager.AddSocket(webSocket);

        // Garder la connexion active tant que le WebSocket est ouvert
        while (webSocket.State == WebSocketState.Open)
        {
            // Lire les messages si nécessaire
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                break; // Terminer la connexion
            }
        }

        // Nettoyer après déconnexion
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

// Mapping des contrôleurs REST&
app.MapControllers();

await app.RunAsync();
