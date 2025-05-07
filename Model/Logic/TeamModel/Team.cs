using ScorePALServer.Model.MatchModel;
using Model.Logic.ClubModel;
using Model.Logic.MatchModel;

namespace ScorePALServer.Model.TeamModel;

public class Team
{
    public long Id { get; set; }

    public String Name { get; set; }

    public Club Club { get; set; }

    public List<IMatch> Matches { get; set; }
}