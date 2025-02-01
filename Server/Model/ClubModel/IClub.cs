using ScorePALServer.Model.Team;

namespace ScorePALServer.Model.ClubModel;

public interface IClub
{
    long Id { get; set; }
    String Name { get; set; }
    String LogoUrl { get; set; }

    List<ITeam> Teams { get; set; }
}