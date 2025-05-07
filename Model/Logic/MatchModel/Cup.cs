using Model.Logic.MatchModel;

namespace ScorePALServer.Model.MatchModel;

public class Cup
{
    private long id;
    private String name;
    private Scale scale;
    private List<MatchCup> matches;

    public long Id
    {
        get => id;
        set => id = value;
    }

    public string Name
    {
        get => name;
        set => name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public Scale Scale
    {
        get => scale;
        set => scale = value;
    }

    public List<MatchCup> Matches
    {
        get => matches;
        set => matches = value ?? throw new ArgumentNullException(nameof(value));
    }
}