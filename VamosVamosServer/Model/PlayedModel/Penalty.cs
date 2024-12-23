namespace VamosVamosServer.Model.PlayedModel;

public class Penalty : IPenalty
{
    private PenaltyObtainingMethod obtainingMethod;
    private PenaltyResult result;
    private int penaltyTime;
    private Played played;

    public PenaltyObtainingMethod ObtainingMethod
    {
        get => obtainingMethod;
        set => obtainingMethod = value;
    }

    public PenaltyResult Result
    {
        get => result;
        set => result = value;
    }

    public int PenaltyTime
    {
        get => penaltyTime;
        set => penaltyTime = value;
    }

    public Played Played
    {
        get => played;
        set => played = value ?? throw new ArgumentNullException(nameof(value));
    }
}