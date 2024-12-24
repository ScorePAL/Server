using System.Net.WebSockets;
using System.Text;

namespace VamosVamosServer.WebSockets;

public class WebSocketManager
{
    private readonly List<WebSocket> _sockets = new();

    public void AddSocket(System.Net.WebSockets.WebSocket socket)
    {
        _sockets.Add(socket);
    }

    public async Task BroadcastMessageAsync(string message)
    {
        var buffer = Encoding.UTF8.GetBytes(message);
        foreach (var socket in _sockets)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
