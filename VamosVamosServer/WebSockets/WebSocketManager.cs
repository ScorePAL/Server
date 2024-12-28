using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace VamosVamosServer.WebSockets;

public class WebSocketManager
{
    private readonly List<WebSocket> _sockets = new();
    private readonly object _lock = new();

    public void AddSocket(WebSocket socket)
    {
        lock (_lock)
        {
            _sockets.Add(socket);
        }
    }

    public async Task BroadcastMessageAsync(string title, Dictionary<string, dynamic> body)
    {
        lock (_lock)
        {
            JsonObject json = new()
            {
                ["type"] = title,
                ["message"] = JsonNode.Parse(JsonConvert.SerializeObject(body))
            };

            var buffer = Encoding.UTF8.GetBytes(json.ToString());
            var toRemove = new List<WebSocket>();

            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    try
                    {
                        socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    catch
                    {
                        toRemove.Add(socket);
                    }
                }
                else
                {
                    toRemove.Add(socket);
                }
            }

            // Retirer les sockets fermés ou en erreur
            foreach (var socket in toRemove)
            {
                _sockets.Remove(socket);
            }
        }
    }
}
