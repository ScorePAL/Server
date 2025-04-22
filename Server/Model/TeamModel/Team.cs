using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.MatchModel;

namespace ScorePALServer.Model.TeamModel;

public class Team
{
    private long id;
    private String name;
    private Club club;
    private List<IMatch> matches;

    public long Id
    {
        get => id;
        set => id = value;
    }

    public String Name
    {
        get => name;
        set => name = value;
    }

    public Club Club
    {
        get => club;
        set => club = value;
    }

    public List<IMatch> Matches
    {
        get => matches;
        set => matches = value;
    }
}