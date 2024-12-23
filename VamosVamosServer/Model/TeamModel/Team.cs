using System;
using VamosVamosServer.Model.ClubModel;
using VamosVamosServer.Model.MatchModel;

namespace VamosVamosServer.Model.Team;

public class Team : ITeam
{
    private int _id;
    private String _name;
    private IClub _club;
    private List<IMatch> _matches;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public String Name
    {
        get => _name;
        set => _name = value;
    }

    public IClub Club
    {
        get => _club;
        set => _club = value;
    }

    public List<IMatch> Matches
    {
        get => _matches;
        set => _matches = value;
    }
}