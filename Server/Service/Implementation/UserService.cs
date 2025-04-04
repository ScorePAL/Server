using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Security;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Service.Implementation;

public class UserService : IUserService
{
    private readonly IUserDAO dao;

    public UserService(IUserDAO dao)
    {
        this.dao = dao;
    }

    public ActionResult GetUserByToken(string token)
    {
        return dao.GetUserByToken(token);
    }

    public ActionResult RegisterUser(string firstName, string lastName, string email, string password, long clubId)
    {
        string salt = GenerateSalt();
        string hashedPassword = HashPassword(password, salt);

        return dao.RegisterUser(firstName, lastName, email, hashedPassword, clubId, salt);
    }

    public ActionResult<Tuple<string, string>> LoginUser(string email, string password)
    {
        string salt = dao.GetSaltBuYser(email);

        if (salt == "")
        {
            return new BadRequestResult();
        }

        var hashedPassword = HashPassword(password, salt);

        return dao.LoginUser(email, hashedPassword);
    }

    public ActionResult<Tuple<string, string>> GenerateNewToken(string refreshtoken)
    {
        return dao.GenerateNewToken(refreshtoken);
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