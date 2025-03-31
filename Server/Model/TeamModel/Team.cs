using ScorePAL.Model.ClubModel;
using ScorePALServer.Model.MatchModel;

namespace ScorePALServer.Model.TeamModel;

public class Team
{
    public long Id { get; set; }

    public String Name { get; set; }

    public Club Club { get; set; }

    public List<IMatch> Matches { get; set; }
}