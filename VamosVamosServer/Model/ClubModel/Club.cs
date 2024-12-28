using VamosVamosServer.Model.Team;

namespace VamosVamosServer.Model.ClubModel;

public class Club : IClub
{
    private int id;
    private String name;
    private String logoUrl;
    private List<ITeam> teams;

    public String LogoUrl
    {
        get => logoUrl;
        set => logoUrl = value;
    }

    public String Name
    {
        get => name;
        set => name = value;
    }

    public int Id
    {
        get => id;
        set => id = value;
    }

    public List<ITeam> Teams
    {
        get => teams;
        set => teams = value;
    }
}