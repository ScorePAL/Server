using System;
using VamosVamosServer.Model.ClubModel;
using VamosVamosServer.Model.MatchModel;

namespace VamosVamosServer.Model.Team;

public interface ITeam
{
    int Id { get; set; }
    String Name { get; set; }
    IClub Club { get; set; }
    List<IMatch> Matches { get; set; }
}