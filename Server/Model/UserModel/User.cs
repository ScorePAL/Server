using ScorePALServer.Model.ClubModel;

namespace ScorePALServer.Model.UserModel;

public class User
{
    private long id;
    private String firstName;
    private String lastName;
    private Role role;
    private DateTime createdAt;
    private Club club;
    private string token;
    private string refreshToken;

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

    public Role Role
    {
        get => role;
        set => role = value;
    }

    public DateTime CreatedAt
    {
        get => createdAt;
        set => createdAt = value;
    }

    public Club Club
    {
        get => club;
        set => club = value;
    }

    public string Token
    {
        get => token;
        set => token = value;
    }

    public string RefreshToken
    {
        get => refreshToken;
        set => refreshToken = value;
    }
}