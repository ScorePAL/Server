using System;
using VamosVamosServer.Model.PlayedModel;

namespace VamosVamosServer.Model.Player;

public interface IPlayer
{
    int Id { get; set; }
    String FirstName { get; set; }
    String LastName { get; set; }
    DateTime BirthDate { get; set; }
    Position[] Positions { get; set; }

    List<IPlayed> Played { get; set; }
}