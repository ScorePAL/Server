using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Security;

namespace VamosVamosServer.Model.Hashing;

public class Hash
{
    public static string GenerateSalt()
    {
        var salt = new byte[32];
        var generator = new SecureRandom();
        generator.NextBytes(salt);
        return Convert.ToBase64String(salt);
    }

    public static byte[] HashPassword(string text)
    {
        HashAlgorithm algorithm = SHA256.Create();
        return algorithm.ComputeHash(Encoding.UTF8.GetBytes(text));
    }

    public static string GenerateJwtToken(string username)
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
            expires: DateTime.Now.AddMonths(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}