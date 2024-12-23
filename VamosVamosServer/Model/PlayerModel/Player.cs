using VamosVamosServer.Model.PlayedModel;

namespace VamosVamosServer.Model.Player;

public class Player : IPlayer
{
    private int _id;
    private String _firstName;
    private String _lastName;
    private DateTime _birthDate;
    private Position[] _positions;
    private List<IPlayed> _played;

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public String FirstName
    {
        get => _firstName;
        set => _firstName = value;
    }

    public String LastName
    {
        get => _lastName;
        set => _lastName = value;
    }

    public DateTime BirthDate
    {
        get => _birthDate;
        set => _birthDate = value;
    }

    public Position[] Positions
    {
        get => _positions;
        set => _positions = value;
    }

    public List<IPlayed> Played
    {
        get => _played;
        set => _played = value;
    }
}