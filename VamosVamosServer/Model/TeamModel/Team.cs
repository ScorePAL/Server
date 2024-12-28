using System;
using VamosVamosServer.Model.ClubModel;
using VamosVamosServer.Model.MatchModel;

namespace VamosVamosServer.Model.Team;

public class Team : ITeam
{
    private int id;
    private String name;
    private IClub club;
    private List<IMatch> matches;

    public int Id
    {
        get => id;
        set => id = value;
    }

    public String Name
    {
        get => name;
        set => name = value;
    }

    public IClub Club
    {
        get => club;
        set => club = value;
    }

    public List<IMatch> Matches
    {
        get => matches;
        set => matches = value;
    }
}