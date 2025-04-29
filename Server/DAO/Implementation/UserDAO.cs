using Microsoft.AspNetCore.Mvc;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Exceptions.User;
using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.UserModel;

namespace ScorePALServer.DAO.Implementation;

public class UserDAO : IUserDAO
{
    public ActionResult RegisterUser(string firstName, string lastName, string email, string password, long clubId,
        string salt)
    {
        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT * FROM users_auth WHERE email = @email",
            new Dictionary<string, object>
            {
                { "@email", email }
            }
        );

        if (result.Rows.Count > 0)
        {
            throw new EmailAlreadyUsedException();
        }

        long userId = conn.ExecuteInsert(
            "INSERT INTO users (first_name, last_name, role, created_at, club) VALUES (@firstName, @lastName, @role, @createdAt, @relatedTo)",
            new Dictionary<string, object>
            {
                { "@firstName", firstName },
                { "@lastName", lastName },
                { "@role", Role.Supporter.ToString() },
                { "@createdAt", DateTime.Now },
                { "@relatedTo", clubId }
            }
        );

        conn.ExecuteInsert(
            "INSERT INTO users_auth (user_id, email, password, salt) VALUES (@id, @email, @password, @salt)",
            new Dictionary<string, object>
            {
                { "@id", userId },
                { "@email", email },
                { "@password", password },
                { "@salt", salt }
            }
        );

        User user = new User
        {
            Id = userId,
            FirstName = firstName,
            LastName = lastName,
            Role = Role.Supporter,
            CreatedAt = DateTime.Now,
            RelatedTo = new Club
            {
                Id = clubId
            }
        };

        return new OkObjectResult(user);
    }

    public ActionResult<User> LoginUser(string email, string password)
    {
        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT user_id, password, salt, first_name, last_name, role, created_at, club, name, logo_url " +
            "FROM users_auth UA " +
            "INNER JOIN users U ON U.user_id = UA.user_id " +
            "INNER JOIN clubs C on C.club_id = U.club " +
            "WHERE email = @email",
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

        var user = new User
        {
            Id = Convert.ToInt64(result.Rows[0]["user_id"]),
            FirstName = result.Rows[0]["first_name"].ToString() ?? "",
            LastName = result.Rows[0]["last_name"].ToString() ?? "",
            Role = Enum.Parse<Role>(result.Rows[0]["role"].ToString() ?? ""),
            CreatedAt = Convert.ToDateTime(result.Rows[0]["created_at"]),
            RelatedTo = new Club
            {
                Id = Convert.ToInt64(result.Rows[0]["related_to"])
            }
        };

        return new OkObjectResult(user);
    }


    public string GetSaltByUser(string email)
    {
        string salt = "";

        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT salt FROM users_auth WHERE email = @email",
            new Dictionary<string, object>
            {
                { "@email", email }
            }
        );

        if (result.Rows.Count > 0)
        {
            salt = result.Rows[0]["salt"].ToString()!;
        }

        return salt;
    }
}