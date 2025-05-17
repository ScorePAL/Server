using ScorePALServer.Model.PlayedModel;
using ScorePALServerModel.Logic.MatchHistoryModel;
using ScorePALServerModel.Logic.TeamModel;

namespace ScorePALServerModel.Logic.MatchModel;

public class Match : IMatch
{
    public long Id { get; set; }

    public Team Team1 { get; set; }

    public Team Team2 { get; set; }

    public DateTime Date { get; set; }

    public string Address { get; set; }

    public string Coach { get; set; }

    public bool IsHome { get; set; }

    public MatchState State { get; set; }

    public DateTime StartedTime { get; set; }

    public long Score1 { get; set; }

    public long Score2 { get; set; }

    public List<MatchHistory> History { get; set; }

    public List<Played> Lineup { get; set; }
}