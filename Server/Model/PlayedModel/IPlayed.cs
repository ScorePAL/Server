using ScorePALServer.Model.MatchModel;
using ScorePALServer.Model.Player;

namespace ScorePALServer.Model.PlayedModel;

public interface IPlayed
{
    long Id { get; set; }
    long JerseyNumber { get; set; }
    long EntryTime { get; set; }
    long ExitTime { get; set; }
    long Goals { get; set; }
    long BlockedShots { get; set; }
    long OnTargetShots { get; set; }
    long OffTargetShots { get; set; }
    bool YellowCard { get; set; }
    bool RedCard { get; set; }
    bool IsInjured { get; set; }
    bool IsCaptain { get; set; }
    List<Position> Positions { get; set; }
    List<IPenalty> Penalties { get; set; }
    List<IAssist> Assists { get; set; }
    IPlayer Player { get; set; }
    IMatch Match { get; set; }
}