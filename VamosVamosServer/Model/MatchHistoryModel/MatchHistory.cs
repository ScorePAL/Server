using VamosVamosServer.Model.MatchModel;

namespace VamosVamosServer.Model.MatchHistoryModel;

public class MatchHistory
{
    public MatchHistory(int time, string additionnalInformations, IMatch match)
    {
        this.time = time;
        this.additionnalInformations = additionnalInformations;
        this.match = match;
    }

    private MatchEvent e;
    private int time;
    private String additionnalInformations;
    private IMatch match;

    public MatchEvent Event
    {
        get => e;
        set => e = value;
    }

    public int Time
    {
        get => time;
        set => time = value;
    }

    public string AdditionnalInformations
    {
        get => additionnalInformations;
        set => additionnalInformations = value ?? throw new ArgumentNullException(nameof(value));
    }

    public IMatch Match
    {
        get => match;
        set => match = value ?? throw new ArgumentNullException(nameof(value));
    }
}