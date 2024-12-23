using VamosVamosServer.Model.Team;

namespace VamosVamosServer.Model.ClubModel;

public class Club : IClub
{
    private int _id;
    private String _name;
    private String _logoUrl;
    private List<ITeam> _teams;

    public String LogoUrl
    {
        get => _logoUrl;
        set => _logoUrl = value;
    }

    public String Name
    {
        get => _name;
        set => _name = value;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public List<ITeam> Teams
    {
        get => _teams;
        set => _teams = value;
    }
}