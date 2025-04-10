using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Exceptions.User;
using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.UserModel;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Service.Implementation;

public class UserService : IUserService
{
    private readonly IUserDAO dao;
    private readonly ITokenService tokenService;

    public UserService(IUserDAO dao, ITokenService tokenService)
    {
        this.dao = dao;
        this.tokenService = tokenService;
    }

    /// <summary>
    /// Return the user based on the given token
    /// </summary>
    /// <returns></returns>
    private User GetUserByToken(ClaimsPrincipal claims)
    {
        if (Enum.TryParse(claims.FindFirst("role")?.Value!, out Role role))
        {
            return new User
            {
                Id = Int64.Parse(claims.FindFirst("id")?.Value!),
                FirstName = claims.FindFirst("firstName")?.Value!,
                LastName = claims.FindFirst("lastName")?.Value!,
                Role = role
            };
        }

        return null;
    }

    public ActionResult RegisterUser(string firstName, string lastName, string email, string password, long clubId)
    {
        string salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);

        return dao.RegisterUser(firstName, lastName, email, hashedPassword, clubId, salt);
    }

    public ActionResult<User> LoginUser(string email, string password)
    {
        string salt = dao.GetSaltByUser(email);

        if (salt == "")
        {
            return new BadRequestResult();
        }

        var hashedPassword = HashPassword(password, salt);

        ActionResult<User> result = dao.LoginUser(email, hashedPassword);

        if (result is OkObjectResult)
        {
            User user = result.Value!;
            var token = tokenService.Create(user);
            var refreshToken = tokenService.CreateRefresh(user);

            user.Token = token;
            user.RefreshToken = refreshToken;

            return new OkObjectResult(user);
        }

        return result;
    }

    public ActionResult<string> GenerateNewToken(ClaimsPrincipal claims)
    {
        User user = GetUserByToken(claims);

        if (user == null)
        {
            throw new UserNotFoundException("Not provided");
        }

        var token = tokenService.CreateRefresh(user);

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
        SHA256 SHAHasher = SHA256.Create();
        var hashed = SHAHasher.ComputeHash(Encoding.UTF8.GetBytes(salt + password));
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