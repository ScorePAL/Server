using ScorePALServerModel.Logic.ClubModel;

namespace ScorePALServer.Model.UserModel;

public class User
{
    public long Id { get; set; }

    public String FirstName { get; set; }

    public String LastName { get; set; }

    public Role Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public Club RelatedTo { get; set; }

    public String Token { get; set; }

    public String RefreshToken { get; set; }
}