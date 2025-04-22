using ScorePALServer.Model.MatchHistoryModel;
using ScorePALServer.Model.PlayedModel;
using ScorePALServer.Model.TeamModel;

namespace ScorePALServer.Model.MatchModel;

public class MatchCup : IMatch
{
    private Cup cup;
    private IMatch match;

    public DateTime StartedTime
    {
        get => match.StartedTime;
        set => match.StartedTime = value;
    }

    public MatchState State
    {
        get => match.State;
        set => match.State = value;
    }

    public String Coach
    {
        get => match.Coach;
        set => match.Coach = value;
    }

    public DateTime Date
    {
        get => match.Date;
        set => match.Date = value;
    }

    public String Address
    {
        get => match.Address;
        set => match.Address = value;
    }

    public long Score2
    {
        get => match.Score2;
        set => match.Score2 = value;
    }

    public Team Team1
    {
        get => match.Team1;
        set => match.Team1 = value;
    }

    public Team Team2
    {
        get => match.Team2;
        set => match.Team2 = value;
    }

    public long Score1
    {
        get => match.Score1;
        set => match.Score1 = value;
    }

    public long Id
    {
        get => match.Id;
        set => match.Id = value;
    }

    public List<MatchHistory> History
    {
        get => match.History;
        set => match.History = value;
    }

    public List<Played> Lineup {
        get => match.Lineup;
        set => match.Lineup = value;
    }

    public Cup Cup
    {
        get => cup;
        set => cup = value;
    }

    public IMatch Match
    {
        get => match;
        set => match = value;
    }


    public MatchCup(Cup cup, IMatch match)
    {
        this.cup = cup;
        this.match = match;
    }

    public MatchCup()
    {
    }
}