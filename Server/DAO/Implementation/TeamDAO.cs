using System.Data;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Exceptions.Team;
using ScorePALServer.Exceptions.User;
using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.TeamModel;
using ScorePALServer.Model.UserModel;

namespace ScorePALServer.DAO.Implementation;

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

    public ActionResult<Team> GetTeam(long id)
    {
        Team team;
        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT team_id, t.name as 'TeamName', c.club_id, c.name, logo_url FROM teams t INNER JOIN clubs c on t.club_id = c.club_id WHERE team_id = @id",
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
        if (user.Club.Id != Convert.ToInt64(row["club_id"]) || user.Role != Role.Staff && user.Role != Role.Admin)
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