using ScorePALServer.Model.TeamModel;

namespace ScorePALServerModel.Logic.ClubModel;

public class Club
{
    public String LogoUrl { get; set; }

    public String Name { get; set; }

    public long Id { get; set; }

    public List<Team> Teams { get; set; }
}