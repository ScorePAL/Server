using ScorePALServer.Model.MatchModel;
using Model.Logic.MatchModel;

namespace Model.Logic.MatchHistoryModel;

public class MatchHistory
{
    public MatchHistory(long time, string additionnalInformations, IMatch match)
    {
        this.time = time;
        this.additionnalInformations = additionnalInformations;
        this.match = match;
    }

    private MatchEvent e;
    private long time;
    private String additionnalInformations;
    private IMatch match;

    public MatchEvent Event
    {
        get => e;
        set => e = value;
    }

    public long Time
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