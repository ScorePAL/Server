namespace ScorePALServer.Model.PlayedModel;

public class Assist
{
    public Played AssistedPlayed { get; set; }

    public AssistType AssistType { get; set; }

    public long AssistTime { get; set; }

    public Played Played { get; set; }
}