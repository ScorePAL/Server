using System.Collections.Concurrent;

namespace ScorePALServerController.SSE;

public interface IConnectionManager
{
    Task AddConnection(long clubId, HttpContext context, CancellationToken token);
    Task RemoveConnection(long clubId, HttpContext context);
    IEnumerable<(HttpContext Context, CancellationToken Token)> GetConnections(long clubId);
}

public class ConnectionManager : IConnectionManager
{
    private readonly ConcurrentDictionary<long, ConcurrentDictionary<HttpContext, CancellationToken>> _connections = new();

    public Task AddConnection(long clubId, HttpContext context, CancellationToken token)
    {
        var connections = _connections.GetOrAdd(clubId, _ =>
            new ConcurrentDictionary<HttpContext, CancellationToken>());
        connections.TryAdd(context, token);
        return Task.CompletedTask;
    }

    public Task RemoveConnection(long clubId, HttpContext context)
    {
        if (_connections.TryGetValue(clubId, out var connections))
        {
            connections.TryRemove(context, out _);
        }
        return Task.CompletedTask;
    }

    public IEnumerable<(HttpContext Context, CancellationToken Token)> GetConnections(long clubId)
    {
        if (_connections.TryGetValue(clubId, out var connections))
        {
            return connections.Select(kvp => (kvp.Key, kvp.Value));
        }
        return Enumerable.Empty<(HttpContext, CancellationToken)>();
    }
}