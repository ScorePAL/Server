namespace ScorePALServerModel.Logic.UserModel;

public record UserRegister
{
    public String FirstName { get; set; }
    public String LastName { get; set; }
    public String Email { get; set; }
    public String Password { get; set; }
    public long ClubId { get; set; }
}