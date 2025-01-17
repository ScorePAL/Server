using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using VamosVamosServer.DAO.Implementation;
using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Model.User;

namespace VamosVamosServer.SSE;

public class SSEController
{
    private ConcurrentDictionary<long, ConcurrentDictionary<HttpContext, CancellationToken>> _subscriptions = new();

    public void Subscribe(long clubId, HttpContext ctx, CancellationToken token)
    {
        var authHeader = ctx.Request.Headers["Authorization"].ToString();
        if (!ValidateJwt(authHeader))
        {
            ctx.Response.StatusCode = 401;
            return;
        }

        var connections = _subscriptions.GetOrAdd(clubId, _ => new ConcurrentDictionary<HttpContext, CancellationToken>());
        connections.TryAdd(ctx, token);
    }

    private bool ValidateJwt(string jwt)
    {
        IUserDAO dao = new UserDAO();
        var user = dao.GetUserByToken(jwt);
        return user is OkObjectResult;
    }


    public void Unsubscribe(long clubId, HttpContext ctx)
    {
        if (_subscriptions.ContainsKey(clubId))
        {
            _subscriptions[clubId].TryRemove(ctx, out _);
        }
    }

    public async Task SendMessage(string message, long clubId)
    {
        if (_subscriptions.TryGetValue(clubId, out var subs))
        {
            await Task.WhenAll(subs.Select(async conn =>
            {
                var ctx = conn.Key;
                var token = conn.Value;

                try
                {
                    if (!token.IsCancellationRequested)
                    {
                        ctx.Response.Headers.Append(HeaderNames.ContentType, "text/event-stream");
                        await ctx.Response.WriteAsync($"data: {message}\n\n", cancellationToken: token);
                        await ctx.Response.Body.FlushAsync(cancellationToken: token);
                    }
                }
                catch
                {
                    subs.TryRemove(ctx, out _); // Supprimer la connexion en cas d'erreur
                }
            }));
        }
    }


}