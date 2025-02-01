using Microsoft.Net.Http.Headers;
using ScorePALServer.SSE.Events;

namespace ScorePALServer.SSE;
public interface IEventPublisher
    {
        Task PublishEvent<TEvent>(TEvent @event, IEnumerable<long> clubIds) where TEvent : class;
    }

    public class SSEEventPublisher : IEventPublisher
    {
        private readonly IConnectionManager _connectionManager;
        private readonly IDictionary<Type, Func<object, Event>> _messageFactories;

        public SSEEventPublisher(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
            _messageFactories = new Dictionary<Type, Func<object, Event>>
            {
                { typeof(ScoreUpdatedEvent), e => new ScoreUpdatedEventMessage((ScoreUpdatedEvent)e) }
            };
        }

        public async Task PublishEvent<TEvent>(TEvent @event, IEnumerable<long> clubIds) where TEvent : class
        {
            if (!_messageFactories.TryGetValue(typeof(TEvent), out var factory))
            {
                throw new ArgumentException($"No message factory registered for event type {@event.GetType()}");
            }

            var message = factory(@event).GetMessage();
            var tasks = clubIds.SelectMany(clubId =>
                _connectionManager.GetConnections(clubId)
                    .Select(conn => SendMessageToClient(conn.Context, conn.Token, message)));

            await Task.WhenAll(tasks);
        }

        private async Task SendMessageToClient(HttpContext context, CancellationToken token, string message)
        {
            try
            {
                if (!token.IsCancellationRequested)
                {
                    context.Response.Headers.Append(HeaderNames.ContentType, "text/event-stream");
                    await context.Response.WriteAsync($"data: {message}\n\n", token);
                    await context.Response.Body.FlushAsync(token);
                }
            }
            catch
            {
                await _connectionManager.RemoveConnection(0, context); // Club ID needs to be tracked
            }
        }
    }