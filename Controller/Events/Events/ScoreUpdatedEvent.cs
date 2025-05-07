using System.Text.Json;
using Model.Logic.MatchModel;
using ScorePALServer.SSE;

namespace ScorePALServerController.Events.Events;

public record ScoreUpdatedEvent(Match Match);

public class ScoreUpdatedEventMessage : Event
{
    private ScoreUpdatedEvent e;

    public ScoreUpdatedEventMessage(ScoreUpdatedEvent @event)
    {
        e = @event;
    }

    public string GetMessage()
    {
        return $"type: 'score_updated', " +
               $"data: {
                   JsonSerializer.Serialize(e)
               }";
    }
}