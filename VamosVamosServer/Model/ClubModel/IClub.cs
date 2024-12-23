using VamosVamosServer.Model.Team;

namespace VamosVamosServer.Model.ClubModel;

public interface IClub
{
    int Id { get; set; }
    String Name { get; set; }
    String LogoUrl { get; set; }

    List<ITeam> Teams { get; set; }
}