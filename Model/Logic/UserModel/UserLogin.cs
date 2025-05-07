namespace Model.Logic.UserModel;

public record UserLogin
{
    public string Email { get; set; }
    public string Password { get; set; }
}