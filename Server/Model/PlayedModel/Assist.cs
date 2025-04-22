namespace ScorePALServer.Model.PlayedModel;

public class Assist
{
    private long id;
    private AssistType assistType = AssistType.Pass;
    private long assistTime;
    private Played played;
    private Played assistedPlayed;

    public Played AssistedPlayed
    {
        get => assistedPlayed;
        set => assistedPlayed = value ?? throw new ArgumentNullException(nameof(value));
    }

    public AssistType AssistType
    {
        get => assistType;
        set => assistType = value;
    }

    public long AssistTime
    {
        get => assistTime;
        set => assistTime = value;
    }

    public Played Played
    {
        get => played;
        set => played = value ?? throw new ArgumentNullException(nameof(value));
    }
}