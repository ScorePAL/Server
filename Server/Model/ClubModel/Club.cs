using ScorePALServer.Model.Team;

namespace ScorePALServer.Model.ClubModel;

public class Club : IClub
{
    private long id;
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

    public long Id
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