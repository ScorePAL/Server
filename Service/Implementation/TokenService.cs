using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.UserModel;
using ScorePALServerService.Interfaces;
using Shared.Configuration;

namespace ScorePALServerService.Implementation;

public class TokenService : ITokenService
{
    private readonly SigningCredentials credentials;

    public TokenService(IOptions<OAuthConfig> oauthOptions)
    {
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(oauthOptions.Value.Key)
        );

        credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }

    public string Create(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("id", user.Id.ToString()),
                new Claim("first_name", user.FirstName),
                new Claim("last_name", user.LastName),
                new Claim("role", user.Role.ToString()),
                new Claim("clubId", user.RelatedTo.Id.ToString())
            ]),
            // Expiration
            Expires = DateTime.UtcNow.AddMinutes(10),

            // Algorithme de signature
            SigningCredentials = credentials,

            // Qui émet le jeton
            Issuer = "https://localhost/",

            // Pour qui est prévu le jeton (qui va l' utiliser pour authentifier un utilisateur)
            Audience = "https://localhost/api"
        };

        return CreateToken(tokenDescriptor);
    }

    public string CreateRefresh(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("id", user.Id.ToString()),
                new Claim("first_name", user.FirstName),
                new Claim("last_name", user.LastName),
                new Claim("role", user.Role.ToString()),
                new Claim("clubId", user.RelatedTo.Id.ToString())
            ]),
            // Expiration
            Expires = DateTime.UtcNow.AddMinutes(1440),

            // Algorithme de signature
            SigningCredentials = credentials,

            // Qui émet le jeton
            Issuer = "https://localhost/",

            // Pour qui est prévu le jeton (qui va l' utiliser pour authentifier un utilisateur)
            Audience = "https://localhost/api/user/refresh-token"
        };

        return CreateToken(tokenDescriptor);
    }

    public string CreateResetPasswordToken(string email)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("email", email),
            ]),
            // Expiration
            Expires = DateTime.UtcNow.AddMinutes(120),

            // Algorithme de signature
            SigningCredentials = credentials,

            // Qui émet le jeton
            Issuer = "https://localhost/",

            // Pour qui est prévu le jeton (qui va l'utiliser pour authentifier un utilisateur)
            Audience = "https://localhost/api/user/reset-password"
        };

        return CreateToken(tokenDescriptor);
    }

    public User ExtractUser(ClaimsPrincipal claims)
    {
        User user = new User
        {
            Id = claims.FindFirst("id") != null ? long.Parse(claims.FindFirst("id")?.Value!) : 0,
            FirstName = claims.FindFirst("first_name")?.Value!,
            LastName = claims.FindFirst("last_name")?.Value!,
            Role = Enum.TryParse(claims.FindFirst("role")?.Value!, out Role role) ? role : Role.Supporter,
            RelatedTo = new Club
            {
                Id = claims.FindFirst("clubId") != null ? long.Parse(claims.FindFirst("clubId")?.Value!) : 0
            }
        };

        return user;
    }

    public void CheckIfUserIsAdminStaffOrCoach(ClaimsPrincipal claims)
    {
        if (!Enum.TryParse(claims.FindFirst("role")?.Value, out Role role))
        {
            throw new UnauthorizedAccessException();
        }

        if (role <= Role.Player)
        {
            throw new UnauthorizedAccessException();
        }
    }

    public void CheckIfUserIsAdmin(ClaimsPrincipal claims)
    {
        if (!Enum.TryParse(claims.FindFirst("role")?.Value, out Role role))
        {
            throw new UnauthorizedAccessException();
        }

        if (role != Role.Admin)
        {
            throw new UnauthorizedAccessException();
        }
    }

    public void CheckIfUserIsAdminOrCoach(ClaimsPrincipal claims)
    {
        if (!Enum.TryParse(claims.FindFirst("role")?.Value, out Role role))
        {
            throw new UnauthorizedAccessException();
        }

        if (role <= Role.Staff)
        {
            throw new UnauthorizedAccessException();
        }
    }

    /// <summary>
    /// Create a token with a given time and object
    /// </summary>
    /// <param name="descriptor"></param>
    /// <exception cref="InvalidDataException"></exception>
    /// <returns></returns>
    private string CreateToken(SecurityTokenDescriptor descriptor)
    {
        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(descriptor);
    }
}