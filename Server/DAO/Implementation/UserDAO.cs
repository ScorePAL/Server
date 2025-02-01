using Microsoft.AspNetCore.Mvc;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.Hashing;
using ScorePALServer.Model.User;

namespace ScorePALServer.DAO.Implementation;

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
            return new BadRequestObjectResult("Token invalide.");
        }

        return new OkObjectResult(user);
    }

    public ActionResult RegisterUser(string firstName, string lastName, string email, string password, long clubId)
    {
        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT * FROM user_authentification WHERE email = @email",
            new Dictionary<string, object>
            {
                { "@email", email }
            }
        );

        if (result.Rows.Count > 0)
        {
            return new BadRequestObjectResult("Un utilisateur avec cet email existe déjà.");
        }

        string salt = Hash.GenerateSalt();
        byte[] hashedPassword = Hash.HashPassword(password + salt);

        long userId = conn.ExecuteInsert(
            "INSERT INTO users (first_name, last_name, role, created_at, related_to) VALUES (@firstName, @lastName, @role, @createdAt, @relatedTo)",
            new Dictionary<string, object>
            {
                { "@firstName", firstName },
                { "@lastName", lastName },
                { "@role", Role.Supporter.ToString() },
                { "@createdAt", DateTime.Now },
                { "@relatedTo", clubId }
            }
        );

        conn.ExecuteInsert("INSERT INTO user_authentification (user_id, email, password, salt) VALUES (@id, @email, @password, @salt)",
            new Dictionary<string, object>
            {
                { "@id", userId },
                { "@email", email },
                { "@password", Convert.ToBase64String(hashedPassword) },
                { "@salt", salt }
            }
        );

        return new OkObjectResult("Utilisateur créé avec succès.");
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
            return new BadRequestObjectResult("Aucun utilisateur avec cet email.");
        }

        byte[] hashedPassword = Convert.FromBase64String(result.Rows[0]["password"].ToString() ?? "");
        string salt = result.Rows[0]["salt"].ToString() ?? "";

        byte[] hashedPasswordToCheck = Hash.HashPassword(password + salt);

        if (!hashedPassword.SequenceEqual(hashedPasswordToCheck))
        {
            return new BadRequestObjectResult("Mot de passe incorrect.");
        }

        string token = Hash.GenerateJwtToken(email, 120);
        string refreshToken = Hash.GenerateJwtToken(email, 150);
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

        if (result.Rows.Count == 0) return new BadRequestObjectResult("Token invalide.");

        string token = Hash.GenerateJwtToken(result.Rows[0]["user_id"].ToString() ?? "", 120);
        string refreshToken = Hash.GenerateJwtToken(result.Rows[0]["user_id"].ToString() ?? "", 150);
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
}