namespace VamosVamosServer.Model.PlayedModel;

public class Assist : IAssist
{
    private int id;
    private AssistType assistType = AssistType.Pass;
    private int assistTime;
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

    public int AssistTime
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