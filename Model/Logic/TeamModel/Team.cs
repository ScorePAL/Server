using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.MatchModel;

namespace ScorePALServerModel.Logic.TeamModel;

public class Team
{
    public long Id { get; set; }

    public String Name { get; set; }

    public Club Club { get; set; }

    public List<IMatch> Matches { get; set; }
}