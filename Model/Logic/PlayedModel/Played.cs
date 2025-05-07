using ScorePALServer.Model.MatchModel;
using ScorePALServer.Model.PlayerModel;
using Model.Logic.MatchModel;

namespace ScorePALServer.Model.PlayedModel;

public class Played
{
    public IMatch Match { get; set; }

    public List<Penalty> Penalties { get; set; }

    public List<Assist> Assists { get; set; }

    public Player Player { get; set; }

    public List<Position> Positions { get; set; }

    public bool IsCaptain { get; set; }

    public bool IsInjured { get; set; }

    public bool RedCard { get; set; }

    public bool YellowCard { get; set; }

    public long OffTargetShots { get; set; }

    public long OnTargetShots { get; set; }

    public long BlockedShots { get; set; }

    public long Goals { get; set; }

    public long ExitTime { get; set; }

    public long EntryTime { get; set; }

    public long JerseyNumber { get; set; }

    public long Id { get; set; }
}