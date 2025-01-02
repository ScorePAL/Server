using System.Net;
using System.Net.WebSockets;
using System.Text;
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

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserDAO, UserDAO>();

var app = builder.Build();

// Swagger Configuration
app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection();
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseAuthorization();

app.UseWebSockets();
app.Map("/ws", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocketManager = app.Services.GetRequiredService<WebSocketManager>();

        // Vérifier et récupérer le clubId
        if (!int.TryParse(context.Request.Query["clubId"], out var clubId))
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await context.Response.WriteAsync("Invalid clubId");
            return;
        }

        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        webSocketManager.AddSocket(clubId, webSocket);

        try
        {
            var buffer = new byte[1024 * 4];
            while (webSocket.State == WebSocketState.Open)
            {
                // Recevoir des messages
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Message reçu de club {clubId}: {message}");

                    // Vous pouvez traiter ou répondre au client ici
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur WebSocket pour club {clubId}: {ex.Message}");
        }
        finally
        {
            webSocketManager.RemoveSocket(clubId, webSocket); // Supprimer après déconnexion
            await webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                "Closing",
                CancellationToken.None
                );
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

// Mapping des contrôleurs REST&
app.MapControllers();

await app.RunAsync();
