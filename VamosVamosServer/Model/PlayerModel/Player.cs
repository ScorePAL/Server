using VamosVamosServer.Model.PlayedModel;

namespace VamosVamosServer.Model.Player;

public class Player : IPlayer
{
    private long id;
    private String firstName;
    private String lastName;
    private DateTime birthDate;
    private Position[] positions;
    private List<IPlayed> played;

    public long Id
    {
        get => id;
        set => id = value;
    }

    public String FirstName
    {
        get => firstName;
        set => firstName = value;
    }

    public String LastName
    {
        get => lastName;
        set => lastName = value;
    }

    public DateTime BirthDate
    {
        get => birthDate;
        set => birthDate = value;
    }

    public Position[] Positions
    {
        get => positions;
        set => positions = value;
    }

    public List<IPlayed> Played
    {
        get => played;
        set => played = value;
    }
}