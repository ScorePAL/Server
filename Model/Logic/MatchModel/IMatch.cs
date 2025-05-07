using ScorePALServer.Model.PlayedModel;
using ScorePALServer.Model.TeamModel;
using Model.Logic.MatchHistoryModel;

namespace Model.Logic.MatchModel;

public interface IMatch
{
    Team Team1 { get; set; }
    Team Team2 { get; set; }
    long Score1 { get; set; }
    long Score2 { get; set; }
    String Address { get; set; }
    DateTime Date { get; set; }
    bool IsHome { get; set; }
    String Coach { get; set; }
    MatchState State { get; set; }
    DateTime StartedTime { get; set; }
    long Id { get; set; }
    List<MatchHistory> History { get; set; }
    List<Played> Lineup { get; set; }
}