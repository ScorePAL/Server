using VamosVamosServer.Model.MatchHistoryModel;
using VamosVamosServer.Model.PlayedModel;
using VamosVamosServer.Model.Team;

namespace VamosVamosServer.Model.MatchModel;

public interface IMatch
{
    ITeam Team1 { get; set; }
    ITeam Team2 { get; set; }
    int Score1 { get; set; }
    int Score2 { get; set; }
    String Address { get; set; }
    DateTime Date { get; set; }
    bool IsHome { get; set; }
    String Coach { get; set; }
    MatchState State { get; set; }
    DateTime StartedTime { get; set; }
    int Id { get; set; }
    List<MatchHistory> History { get; set; }
    List<IPlayed> Lineup { get; set; }
}