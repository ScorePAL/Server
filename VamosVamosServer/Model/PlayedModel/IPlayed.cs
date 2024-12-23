using VamosVamosServer.Model.MatchModel;
using VamosVamosServer.Model.Player;

namespace VamosVamosServer.Model.PlayedModel;

public interface IPlayed
{
    int Id { get; set; }
    int JerseyNumber { get; set; }
    int EntryTime { get; set; }
    int ExitTime { get; set; }
    int Goals { get; set; }
    int BlockedShots { get; set; }
    int OnTargetShots { get; set; }
    int OffTargetShots { get; set; }
    bool YellowCard { get; set; }
    bool RedCard { get; set; }
    bool IsInjured { get; set; }
    bool IsCaptain { get; set; }
    Position Position { get; set; }
    List<IPenalty> Penalties { get; set; }
    List<IAssist> Assists { get; set; }
    IPlayer Player { get; set; }
    IMatch Match { get; set; }
}