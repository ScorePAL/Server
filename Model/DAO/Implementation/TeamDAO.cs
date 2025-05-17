using System.Data;
using Microsoft.Extensions.DependencyInjection;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServerModel.Exceptions.Team;
using ScorePALServerModel.Exceptions.User;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServer.Model.TeamModel;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Exceptions.Club;

namespace ScorePALServerModel.DAO.Implementation;

public class TeamDAO : ITeamDAO
{

    private readonly IServiceProvider provider;
    public TeamDAO(IServiceProvider provider)
    {
        this.provider = provider;
    }

    public Team[] GetTeams(long page, long limit)
    {

        List<Team> teams = new List<Team>();


        using var scope = provider.CreateScope();
        var conn = scope.ServiceProvider.GetRequiredService<MySqlController>();
        var result = conn.ExecuteQuery(
            "SELECT team_id, t.name, c.club_id, c.name, logo_url " +
            "FROM teams t " +
            "INNER JOIN clubs c on t.club_id = c.club_id " +
            "LIMIT @limit OFFSET @offset",
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

        return teams.ToArray();
    }


    public Team GetTeam(Team team)
    {
        Team teamResult;

        using var scope = provider.CreateScope();
        var conn = scope.ServiceProvider.GetRequiredService<MySqlController>();
        var result = conn.ExecuteQuery(
            "SELECT team_id, t.name as 'TeamName', c.club_id, c.name, logo_url " +
            "FROM teams t " +
            "INNER JOIN clubs c on t.club_id = c.club_id " +
            "WHERE team_id = @id",
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

        return teamResult;
    }

    public Team CreateTeam(string name, Club club)
    {

        using var scope = provider.CreateScope();
        var conn = scope.ServiceProvider.GetRequiredService<MySqlController>();
        var result = conn.ExecuteQuery("SELECT club_id FROM clubs WHERE club_id = @id",
            new Dictionary<string, object>
            {
                { "@id", club.Id }
            });

        if (result.Rows.Count == 0) throw new ClubNotFoundException(club.Id);

        conn.ExecuteQuery("INSERT INTO teams (name, club_id) VALUES (@name, @clubId)",
            new Dictionary<string, object>
            {
                { "@name", name },
                { "@clubId", club.Id }
            });

        return new Team
        {
            Name = name,
            Club = club
        };
    }

    public Team UpdateTeam(User user, Team team)
    {

        using var scope = provider.CreateScope();
        var conn = scope.ServiceProvider.GetRequiredService<MySqlController>();
        var result = conn.ExecuteQuery("SELECT club_id FROM teams WHERE team_id = @id",
            new Dictionary<string, object>
            {
                { "@id", team.Id }
            });

        if (result.Rows.Count == 0)
        {
            throw new TeamNotFoundException(team.Id);
        }

        DataRow row = result.Rows[0];
        if (user.RelatedTo.Id != Convert.ToInt64(row["club_id"]) || user.Role != Role.Staff && user.Role != Role.Admin)
        {
            throw new InvalidPermissionsException("You do not have permission to update this team");
        }

        conn.ExecuteQuery("UPDATE teams SET name = @name WHERE team_id = @id",
            new Dictionary<string, object>
            {
                { "@name", team.Name },
                { "@id", team.Id }
            });

        return team;
    }
}