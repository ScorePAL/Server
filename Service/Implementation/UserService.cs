using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServerModel.Logic.UserModel;
using Org.BouncyCastle.Security;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Exceptions.User;
using ScorePALServerService.Interfaces;

namespace ScorePALServerService.Implementation;

public class UserService(IUserDAO dao, ITokenService tokenService) : IUserService
{
    public User RegisterUser(UserRegister userRegister)
    {
        string salt = GenerateSalt();
        string hashedPassword = HashPassword(userRegister.Password, salt);

        userRegister.Password = hashedPassword;


        User user = dao.RegisterUser(userRegister, salt);

        string token = tokenService.Create(user);
        string refreshToken = tokenService.CreateRefresh(user);
        user.Token = token;
        user.RefreshToken = refreshToken;

        return user;
    }

    public User LoginUser(UserLogin userLogin)
    {
        string salt = dao.GetSaltByUser(userLogin.Email);

        if (salt == "")
        {
            throw new UserNotFoundException(userLogin.Email);
        }

        var hashedPassword = HashPassword(userLogin.Password, salt);
        userLogin.Password = hashedPassword;

        User user = dao.LoginUser(userLogin);

        string token = tokenService.Create(user);
        string refreshToken = tokenService.CreateRefresh(user);

        user.Token = token;
        user.RefreshToken = refreshToken;

        return user;
    }

    public ActionResult<string> GenerateNewToken(ClaimsPrincipal claims)
    {
        var user = tokenService.ExtractUser(claims);

        string token = tokenService.CreateRefresh(user);
        return token;
    }

    public ActionResult ResetPassword(string email)
    {
        var token = tokenService.CreateResetPasswordToken(email);

        // TODO : Send email

        return new OkObjectResult(token);
    }


    /// <summary>
    /// Create a hash from a string
    /// </summary>
    /// <param name="password">The password to hash</param>
    /// <param name="salt">The salt to combine with the password</param>
    /// <returns>The hashed password</returns>
    private string HashPassword(string password, string salt)
    {
        SHA256 shaHasher = SHA256.Create();
        var hashed = shaHasher.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
        return Convert.ToBase64String(hashed);
    }

    /// <summary>
    /// Generate a salt for a user
    /// </summary>
    /// <returns></returns>
    private static string GenerateSalt()
    {
        var salt = new byte[32];
        var generator = new SecureRandom();
        generator.NextBytes(salt);
        return Convert.ToBase64String(salt);
    }
}