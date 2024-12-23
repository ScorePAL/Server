using VamosVamosServer.Model.MatchModel;
using VamosVamosServer.Model.Player;

namespace VamosVamosServer.Model.PlayedModel;

public class Played : IPlayed
{
    private int id;
    private Position position;
    private bool isCaptain;
    private bool isInjured;
    private bool redCard;
    private bool yellowCard;
    private int offTargetShots;
    private int onTargetShots;
    private int blockedShots;
    private int goals;
    private int exitTime;
    private int entryTime;
    private int jerseyNumber;
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

    public Position Position
    {
        get => position;
        set => position = value;
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

    public int OffTargetShots
    {
        get => offTargetShots;
        set => offTargetShots = value;
    }

    public int OnTargetShots
    {
        get => onTargetShots;
        set => onTargetShots = value;
    }

    public int BlockedShots
    {
        get => blockedShots;
        set => blockedShots = value;
    }

    public int Goals
    {
        get => goals;
        set => goals = value;
    }

    public int ExitTime
    {
        get => exitTime;
        set => exitTime = value;
    }

    public int EntryTime
    {
        get => entryTime;
        set => entryTime = value;
    }

    public int JerseyNumber
    {
        get => jerseyNumber;
        set => jerseyNumber = value;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }
}