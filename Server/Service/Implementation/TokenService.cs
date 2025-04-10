using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.UserModel;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Service.Implementation;

public class TokenService : ITokenService
{
    public string Create(User user)
    {
        return CreateToken(10, user);
    }

    public string CreateRefresh(User user)
    {
        return CreateToken(1440, user);
    }

    public User ExtractUser(ClaimsPrincipal claims)
    {
        User user = new User
        {
            Id = claims.FindFirst("id") != null ? Int64.Parse(claims.FindFirst("id")?.Value!) : 0,
            FirstName = claims.FindFirst("first_name")?.Value!,
            LastName = claims.FindFirst("last_name")?.Value!,
            Role = Enum.TryParse(claims.FindFirst("role")?.Value!, out Role role) ? role : Role.Supporter,
            RelatedTo = new Club
            {
                Id = claims.FindFirst("clubId") != null ? Int64.Parse(claims.FindFirst("clubId")?.Value!) : 0
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

    private string CreateToken(int minutes, User user)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // racine du projet
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                optional: true)
            .AddEnvironmentVariables()
            .Build();

        //On en fait une clé de crypto symétrique
        SymmetricSecurityKey securityKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("OAuth")["Key"]!));

        //On crée l'algo de signature (en précisant la clé et l'algo utilisé )
        SigningCredentials credentials = new SigningCredentials(securityKey,
            SecurityAlgorithms.HmacSha256);

        //On décrit le token
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            // Info du jeton ( PAYLOAD )
            Subject = new ClaimsIdentity(
                [
                    new Claim("id", user.Id.ToString()),
                    new Claim("first_name", user.FirstName),
                    new Claim("last_name", user.LastName),
                    new Claim("club_id", user.RelatedTo.Id.ToString()),
                    new Claim("role", user.Role.ToString()),
                ]
            ),

            // Expiration
            Expires = DateTime.UtcNow.AddMinutes(minutes),

            // Algorithme de signature
            SigningCredentials = credentials,

            // Qui émet le jeton
            Issuer = "https :// localhost /",

            // Pour qui est prévu le jeton (qui va l' utiliser pour authentifier un utilisateur)
            Audience = "https :// localhost / Commandes "
        };
        // Création du générateur de token
        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(tokenDescriptor);
    }
}