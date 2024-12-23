namespace VamosVamosServer.Model.MatchModel;

public class Cup
{
    private int id;
    private String name;
    private Scale scale;
    private List<MatchCup> matches;

    public int Id
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