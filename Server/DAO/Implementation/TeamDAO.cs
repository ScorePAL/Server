using System.Data;
using Microsoft.AspNetCore.Mvc;
using ScorePAL.Model.ClubModel;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Exceptions.Team;
using ScorePALServer.Exceptions.User;
using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.Team;
using ScorePALServer.Model.User;
using ScorePALServer.Model.TeamModel;
using ScorePALServer.Model.UserModel;

namespace ScorePALServer.DAO.Implementation;

public class TeamDAO : ITeamDAO
{
    public ActionResult<List<Team>> GetTeams(string token, long page, long limit)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult user)
        {
            throw new InvalidTokenException(token);
        }

        List<Team> teams = new List<Team>();

        if (user.Value == null)
        {
            throw new UndefinedUserException();
        }

        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT team_id, team.name, club.club_id, club.name, logo_url FROM team INNER JOIN club on team.club_id = club.club_id LIMIT @limit OFFSET @offset",
            new Dictionary<string, object>
            {
                { "@limit", limit },
                { "@offset", (page - 1) * limit }
            });

        foreach (DataRow row in result.Rows)
        {
            teams.Add(new Team
            {
                Id = Convert.ToInt32(row["team_id"]),
                Name = row["team.name"].ToString() ?? "",
                Club = new Club
                {
                    Id = Convert.ToInt32(row["club_id"]),
                    Name = row["club.name"].ToString() ?? "",
                    LogoUrl = row["logo_url"].ToString() ?? ""
                }
            });
        }

        return new OkObjectResult(teams);
    }

    public ActionResult<Team> GetTeam(string token, long id)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult user)
        {
            throw new InvalidTokenException(token);
        }

        if (user.Value == null)
        {
            throw new UndefinedUserException();
        }

        Team team;
        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT team_id, team.name as 'TeamName', club.club_id, club.name, logo_url FROM team INNER JOIN club on team.club_id = club.club_id WHERE team_id = @id",
            new Dictionary<string, object>
            {
                { "@id", id }
            });

        if (result.Rows.Count <= 0)
        {
            throw new TeamNotFoundException(id);

        }

        DataRow row = result.Rows[0];
        team = new Team
        {
            Id = Convert.ToInt32(row["team_id"]),
            Name = row["TeamName"].ToString() ?? "",
            Club = new Club
            {
                Id = Convert.ToInt32(row["club_id"]),
                Name = row["name"].ToString() ?? "",
                LogoUrl = row["logo_url"].ToString() ?? ""
            }
        };

        return new OkObjectResult(team);
    }

    public ActionResult CreateTeam(string token, string name, long clubId)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult user)
        {
            throw new InvalidTokenException(token);
        }

        if (user.Value == null)
        {
            throw new UndefinedUserException();
        }

        User u = (User)user.Value;
        if (u.RelatedTo.Id != clubId || u.Role != Role.Staff && u.Role != Role.Admin)
        {
            return new UnauthorizedResult();
        }

        using MySqlController conn = new MySqlController();
        conn.ExecuteQuery("INSERT INTO team (name, club_id) VALUES (@name, @clubId)", new Dictionary<string, object>
        {
            { "@name", name },
            { "@clubId", clubId }
        });

        return new OkResult();
    }

    public ActionResult UpdateTeam(string token, long id, string name)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult user)
        {
            throw new InvalidTokenException(token);
        }

        if (user.Value == null)
        {
            throw new UndefinedUserException();
        }

        User u = (User)user.Value;

        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery("SELECT club_id FROM team WHERE team_id = @id",
            new Dictionary<string, object>
            {
                { "@id", id }
            });

        if (result.Rows.Count == 0)
        {
            throw new TeamNotFoundException(id);
        }

        DataRow row = result.Rows[0];
        if (u.RelatedTo.Id != Convert.ToInt64(row["club_id"]) || u.Role != Role.Staff && u.Role != Role.Admin)
        {
            throw new InvalidPermissionsException("You do not have permission to update this team");
        }

        conn.ExecuteQuery("UPDATE team SET name = @name WHERE team_id = @id",
            new Dictionary<string, object>
            {
                { "@name", name },
                { "@id", id }
            });

        return new OkResult();
    }

    public ActionResult DeleteTeam(string token, long id)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult user)
        {
            throw new InvalidTokenException(token);
        }

        if (user.Value == null)
        {
            throw new UndefinedUserException();
        }

        User u = (User)user.Value;

        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery("SELECT * FROM team WHERE team_id = @id", new Dictionary<string, object>
        {
            { "@id", id }
        });

        if (result.Rows.Count == 0)
        {
            throw new TeamNotFoundException(id);
        }

        DataRow row = result.Rows[0];

        if (u.RelatedTo.Id != Convert.ToInt64(row["club_id"]) || u.Role != Role.Staff && u.Role != Role.Admin)
        {
            throw new InvalidPermissionsException("You do not have permission to delete this team");
        }

        conn.ExecuteQuery("DELETE FROM team WHERE team_id = @id", new Dictionary<string, object>
        {
            { "@id", id }
        });

        return new OkResult();
    }
}