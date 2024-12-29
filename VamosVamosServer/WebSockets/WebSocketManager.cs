using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft.Json;

namespace VamosVamosServer.WebSockets;

public class WebSocketManager
{
    private readonly Dictionary<long, List<WebSocket>> socketsByClubId = new();
    private readonly object @lock = new();

    public void AddSocket(long clubId, WebSocket socket)
    {
        lock (@lock)
        {
            if (socketsByClubId.ContainsKey(clubId) == false)
            {
                socketsByClubId[clubId] = new List<WebSocket>();
            }

            socketsByClubId[clubId].Add(socket);
        }
    }

    public Task BroadcastMessageAsync(long clubId, string title, Dictionary<string, dynamic> body)
    {
        lock (@lock)
        {
            JsonObject json = new()
            {
                ["type"] = title,
                ["message"] = JsonNode.Parse(JsonConvert.SerializeObject(body))
            };

            var buffer = Encoding.UTF8.GetBytes(json.ToString());
            var toRemove = new List<WebSocket>();

            foreach (var socket in socketsByClubId[clubId])
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
                socketsByClubId[clubId].Remove(socket);
            }
        }

        return Task.CompletedTask;
    }
}
