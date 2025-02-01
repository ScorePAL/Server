using System;
using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.MatchModel;

namespace ScorePALServer.Model.Team;

public interface ITeam
{
    long Id { get; set; }
    String Name { get; set; }
    IClub Club { get; set; }
    List<IMatch> Matches { get; set; }
}