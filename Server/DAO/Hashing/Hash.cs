using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Security;

namespace ScorePALServer.Model.Hashing;

public class Hash
{
    /// <summary>
    /// Generate a salt for a user
    /// </summary>
    /// <returns></returns>
    public static string GenerateSalt()
    {
        var salt = new byte[32];
        var generator = new SecureRandom();
        generator.NextBytes(salt);
        return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// Create a hash from a string
    /// </summary>
    /// <param name="text">The password to hash</param>
    /// <returns>The hashed password</returns>
    public static byte[] HashPassword(string text)
    {
        HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
    }

    /// <summary>
    /// Generate a JWT token for a user
    /// </summary>
    /// <param name="username">The user's username</param>
    /// <param name="durationInDays">The duration of the token</param>
    /// <returns>The token the was created</returns>
    public static string GenerateJwtToken(string username, int durationInDays)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey("YThSikmVXQ2WAQWRIAUm6iRr5aXMR4Sf"u8.ToArray());
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "S3_A2_LesPloucs",
            audience: "S3_A2_LesPloucs",
            claims: claims,
            expires: DateTime.Now.AddDays(durationInDays),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}