using ScorePALServer.Model.MatchModel;
using ScorePALServer.Model.Player;

namespace ScorePALServer.Model.PlayedModel;

public class Played : IPlayed
{
    private long id;
    private List<Position> positions;
    private bool isCaptain;
    private bool isInjured;
    private bool redCard;
    private bool yellowCard;
    private long offTargetShots;
    private long onTargetShots;
    private long blockedShots;
    private long goals;
    private long exitTime;
    private long entryTime;
    private long jerseyNumber;
    private List<IPenalty> penalties = new();
    private List<IAssist> assists = new();
    private IPlayer player;
    private IMatch match;

    public IMatch Match
    {
        get => match;
        set => match = value;
    }

    public List<IPenalty> Penalties
    {
        get => penalties;
        set => penalties = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<IAssist> Assists
    {
        get => assists;
        set => assists = value ?? throw new ArgumentNullException(nameof(value));
    }

    public IPlayer Player
    {
        get => player;
        set => player = value ?? throw new ArgumentNullException(nameof(value));
    }

    public List<Position> Positions
    {
        get => positions;
        set => positions = value;
    }

    public bool IsCaptain
    {
        get => isCaptain;
        set => isCaptain = value;
    }

    public bool IsInjured
    {
        get => isInjured;
        set => isInjured = value;
    }

    public bool RedCard
    {
        get => redCard;
        set => redCard = value;
    }

    public bool YellowCard
    {
        get => yellowCard;
        set => yellowCard = value;
    }

    public long OffTargetShots
    {
        get => offTargetShots;
        set => offTargetShots = value;
    }

    public long OnTargetShots
    {
        get => onTargetShots;
        set => onTargetShots = value;
    }

    public long BlockedShots
    {
        get => blockedShots;
        set => blockedShots = value;
    }

    public long Goals
    {
        get => goals;
        set => goals = value;
    }

    public long ExitTime
    {
        get => exitTime;
        set => exitTime = value;
    }

    public long EntryTime
    {
        get => entryTime;
        set => entryTime = value;
    }

    public long JerseyNumber
    {
        get => jerseyNumber;
        set => jerseyNumber = value;
    }

    public long Id
    {
        get => id;
        set => id = value;
    }
}