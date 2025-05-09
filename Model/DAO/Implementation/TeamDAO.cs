using System.Data;
using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServerModel.Exceptions.Team;
using ScorePALServerModel.Exceptions.User;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServer.Model.TeamModel;
using ScorePALServer.Model.UserModel;

namespace ScorePALServerModel.DAO.Implementation;

public class TeamDAO : ITeamDAO
{
    public ActionResult<Team[]> GetTeams(long page, long limit)
    {

        List<Team> teams = new List<Team>();

        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT team_id, t.name, c.club_id, c.name, logo_url FROM teams t INNER JOIN clubs c on t.club_id = c.club_id LIMIT @limit OFFSET @offset",
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


    public ActionResult<Team> GetTeam(string token, Team team)
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

        Team teamResult;
        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT team_id, t.name as 'TeamName', c.club_id, c.name, logo_url FROM teams t INNER JOIN clubs c on t.club_id = c.club_id WHERE team_id = @id",
            new Dictionary<string, object>
            {
                { "@id", team.Id }
            });

        if (result.Rows.Count <= 0)
        {
            throw new TeamNotFoundException(team.Id);

        }

        DataRow row = result.Rows[0];
        teamResult = new Team
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

        return new OkObjectResult(teamResult);
    }

    public ActionResult UpdateTeam(User user, long id, string name)
    {
        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery("SELECT club_id FROM teams WHERE team_id = @id",
            new Dictionary<string, object>
            {
                { "@id", id }
            });

        if (result.Rows.Count == 0)
        {
            throw new TeamNotFoundException(id);
        }

        DataRow row = result.Rows[0];
        if (user.RelatedTo.Id != Convert.ToInt64(row["club_id"]) || user.Role != Role.Staff && user.Role != Role.Admin)
        {
            throw new InvalidPermissionsException("You do not have permission to update this team");
        }

        conn.ExecuteQuery("UPDATE teams SET name = @name WHERE team_id = @id",
            new Dictionary<string, object>
            {
                { "@name", name },
                { "@id", id }
            });

        return new OkResult();
    }
}