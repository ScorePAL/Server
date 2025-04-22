using ScorePALServer.Model.TeamModel;

namespace ScorePALServer.Model.ClubModel;

public class Club
{
    private long id;
    private String name;
    private String logoUrl;
    private List<Team> teams;

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

    public List<Team> Teams
    {
        get => teams;
        set => teams = value;
    }
}