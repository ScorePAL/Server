using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.DAO.Interfaces;
using Model.Exceptions.User;
using Model.Logic.ClubModel;
using Model.Logic.UserModel;
using ScorePALServer.Model.UserModel;

namespace Model.DAO.Implementation;

public class UserDAO : IUserDAO
{
    public ActionResult GetUserByToken(string token)
    {
        User? user;

        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT u.user_id, first_name, last_name, role, created_at, related_to, token, club_id, name, logo_url FROM users u INNER JOIN user_tokens ut on u.user_id = ut.user_id INNER JOIN VamosVamos.club c on u.related_to = c.club_id WHERE ut.token = @token",
            new Dictionary<string, object>
            {
                { "@token", token }
            }
        );

        if (result.Rows.Count > 0)
        {
            user = new User();
            user.Id = Convert.ToInt32(result.Rows[0]["user_id"]);
            user.FirstName = result.Rows[0]["first_name"].ToString() ?? "";
            user.LastName = result.Rows[0]["last_name"].ToString() ?? "";

            string r = result.Rows[0]["role"].ToString() ?? "";
            Enum.TryParse(r, out Role role);
            user.Role = role;

            user.CreatedAt = Convert.ToDateTime(result.Rows[0]["created_at"]);
            user.RelatedTo = new Club
            {
                Id = Convert.ToInt32(result.Rows[0]["related_to"]),
                Name = result.Rows[0]["name"].ToString() ?? "",
                LogoUrl = result.Rows[0]["logo_url"].ToString() ?? ""
            };
        }
        else
        {
            throw new InvalidTokenException(token);
        }

        return new OkObjectResult(user);
    }

    public ActionResult RegisterUser(UserRegister userRegister, string salt)
    {
        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT * FROM user_authentification WHERE email = @email",
            new Dictionary<string, object>
            {
                { "@email", userRegister.Email }
            }
        );

        if (result.Rows.Count > 0)
        {
            throw new EmailAlreadyUsedException();
        }

        long userId = conn.ExecuteInsert(
            "INSERT INTO users (first_name, last_name, role, created_at, related_to) VALUES (@firstName, @lastName, @role, @createdAt, @relatedTo)",
            new Dictionary<string, object>
            {
                { "@firstName", userRegister.FirstName },
                { "@lastName", userRegister.LastName },
                { "@role", Role.Supporter.ToString() },
                { "@createdAt", DateTime.Now },
                { "@relatedTo", userRegister.ClubId }
            }
        );

        conn.ExecuteInsert("INSERT INTO user_authentification (user_id, email, password, salt) VALUES (@id, @email, @password, @salt)",
            new Dictionary<string, object>
            {
                { "@id", userId },
                { "@email", userRegister.Email },
                { "@password", userRegister.Password },
                { "@salt", salt }
            }
        );

        return new OkResult();
    }

    public ActionResult<Tuple<String, String>> LoginUser(string email, string password)
    {
        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT user_id, password, salt FROM user_authentification WHERE email = @email",
            new Dictionary<string, object>
            {
                { "@email", email }
            }
        );

        if (result.Rows.Count == 0)
        {
            throw new UserNotFoundException(email);
        }

        string hashedPassword = result.Rows[0]["password"].ToString() ?? "";

        if (!hashedPassword.SequenceEqual(password))
        {
            throw new InvalidPasswordException();
        }

        string token = GenerateJwtToken(email, 10);
        string refreshToken = GenerateJwtToken(email, 45);
        conn.ExecuteInsert("INSERT INTO user_tokens (user_id, token, refresh_token) VALUES (@id, @token, @refreshToken)",
            new Dictionary<string, object>
            {
                { "@id", Convert.ToInt32(result.Rows[0]["user_id"]) },
                { "@token", token },
                { "@refreshToken", refreshToken }
            }
        );

        return new OkObjectResult(new Tuple<string, string>(token, refreshToken));
    }

    public ActionResult<Tuple<string, string>> GenerateNewToken(string refreshtoken)
    {
        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT user_id FROM user_tokens WHERE refresh_token=@refreshToken",
            new Dictionary<string, object>
            {
                { "@refreshToken", refreshtoken }
            });

        if (result.Rows.Count == 0)
        {
            throw new InvalidTokenException(refreshtoken);
        }

        string token = GenerateJwtToken(result.Rows[0]["user_id"].ToString() ?? "", 120);
        string refreshToken = GenerateJwtToken(result.Rows[0]["user_id"].ToString() ?? "", 150);
        conn.ExecuteQuery("UPDATE user_tokens SET token=@token, refresh_token=@refreshToken WHERE refresh_token=@previousRefreshToken AND user_id=@id",
            new Dictionary<string, object>
            {
                { "@token", token },
                { "@refreshToken", refreshToken },
                { "@previousRefreshToken", refreshtoken },
                { "@id", Convert.ToInt32(result.Rows[0]["user_id"]) }
            });

        return new OkObjectResult(new Tuple<string, string>(token, refreshToken));
    }

    public string GetSaltBuYser(string email)
    {
        string salt = "";

        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT salt FROM user_authentification WHERE email = @email",
            new Dictionary<string, object>
            {
                { "@email", email }
            }
        );

        if (result.Rows.Count > 0)
        {
            salt = result.Rows[0]["salt"].ToString();
        }

        return salt;
    }

    /// <summary>
    /// Generate a JWT token for a user
    /// </summary>
    /// <param name="username">The user's username</param>
    /// <param name="durationInDays">The duration of the token</param>
    /// <returns>The token the was created</returns>
    private static string GenerateJwtToken(string username, int durationInDays)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("OAUTHKEY")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "ScorePAL",
            audience: "ScorePAL",
            claims: claims,
            expires: DateTime.Now.AddDays(durationInDays),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}