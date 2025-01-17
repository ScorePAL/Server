using System.Text.Json;

namespace VamosVamosServer.SSE.Events;

public record ScoreUpdatedEvent(long MatchId, int ScoreTeam1, int ScoreTeam2);

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