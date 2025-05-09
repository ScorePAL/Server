using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServerModel.Exceptions.User;
using ScorePALServerModel.Logic.UserModel;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Logic.ClubModel;

namespace ScorePALServerModel.DAO.Implementation;

public class UserDAO : IUserDAO
{
    public User RegisterUser(UserRegister userRegister, string salt)
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

        var user = new User
        {
            Id = userId,
            FirstName = userRegister.FirstName,
            LastName = userRegister.LastName,
            Role = Role.Supporter,
            CreatedAt = DateTime.Now,
            RelatedTo = new Club
            {
                Id = userRegister.ClubId
            }
        };

        return user;
    }

    public User LoginUser(UserLogin userLogin)
    {
        using var conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT UA.user_id, password, salt, first_name, last_name, role, created_at, club, name, logo_url " +
            "FROM users_auth UA " +
            "INNER JOIN users U ON U.user_id = UA.user_id " +
            "INNER JOIN clubs C on C.club_id = U.club " +
            "WHERE email = @email",
            new Dictionary<string, object>
            {
                { "@email", userLogin.Email }
            }
        );

        if (result.Rows.Count == 0)
        {
            throw new UserNotFoundException(userLogin.Email);
        }

        string hashedPassword = result.Rows[0]["password"].ToString() ?? "";

        if (!hashedPassword.SequenceEqual(userLogin.Password))
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

        return user;
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