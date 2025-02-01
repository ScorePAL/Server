using System;
using ScorePALServer.Model.PlayedModel;

namespace ScorePALServer.Model.Player;

public interface IPlayer
{
    long Id { get; set; }
    String FirstName { get; set; }
    String LastName { get; set; }
    DateTime BirthDate { get; set; }
    Position[] Positions { get; set; }

    List<IPlayed> Played { get; set; }
}