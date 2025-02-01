using ScorePALServer.Model.MatchHistoryModel;
using ScorePALServer.Model.PlayedModel;
using ScorePALServer.Model.Team;

namespace ScorePALServer.Model.MatchModel;

public class Match : IMatch
{
    private long id;
    private ITeam team1;
    private ITeam team2;
    private DateTime date;
    private String address;
    private String coach;
    private bool isHome;
    private MatchState state;
    private DateTime startedTime;
    private long score1;
    private long score2;
    private List<MatchHistory> history = new();
    private List<IPlayed> lineup = new();

    public long Id
    {
        get => id;
        set => id = value;
    }

    public ITeam Team1
    {
        get => team1;
        set => team1 = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ITeam Team2
    {
        get => team2;
        set => team2 = value ?? throw new ArgumentNullException(nameof(value));
    }

    public DateTime Date
    {
        get => date;
        set => date = value;
    }

    public string Address
    {
        get => address;
        set => address = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Coach
    {
        get => coach;
        set => coach = value ?? throw new ArgumentNullException(nameof(value));
    }

    public bool IsHome
    {
        get => isHome;
        set => isHome = value;
    }

    public MatchState State
    {
        get => state;
        set => state = value;
    }

    public DateTime StartedTime
    {
        get => startedTime;
        set => startedTime = value;
    }

    public long Score1
    {
        get => score1;
        set => score1 = value;
    }

    public long Score2
    {
        get => score2;
        set => score2 = value;
    }

    public List<MatchHistory> History
    {
        get => history;
        set => history = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<IPlayed> Lineup {
        get => lineup;
        set => lineup = value ?? throw new ArgumentNullException(nameof(value));
    }
}