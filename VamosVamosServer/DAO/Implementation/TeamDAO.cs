using System.Data;
using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Model.ClubModel;
using VamosVamosServer.Model.Team;
using VamosVamosServer.Model.User;

namespace VamosVamosServer.DAO.Implementation;

public class TeamDAO : ITeamDAO
{
    public ActionResult<List<Team>> GetTeams(string token, long page, long limit)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult)
        {
            return new UnauthorizedResult();
        }

        List<Team> teams = new List<Team>();

        OkObjectResult user = (OkObjectResult)r;
        if (user.Value != null)
        {
            using (MySQLController conn = new MySQLController())
            {
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
            }
        }

        return new OkObjectResult(teams);
    }

    public ActionResult<Team> GetTeam(string token, long id)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult)
        {
            return new UnauthorizedResult();
        }


        OkObjectResult user = (OkObjectResult)r;
        if (user.Value == null)
        {
            return new UnauthorizedResult();
        }

        Team team;
        using (MySQLController conn = new MySQLController())
        {
            var result = conn.ExecuteQuery(
                "SELECT team_id, team.name as 'TeamName', club.club_id, club.name, logo_url FROM team INNER JOIN club on team.club_id = club.club_id WHERE team_id = @id",
                new Dictionary<string, object>
                {
                    { "@id", id }
                });

            if (result.Rows.Count > 0)
            {
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
            }
            else
            {
                return new NotFoundResult();
            }
        }

        return new OkObjectResult(team);
    }

    public ActionResult CreateTeam(string token, string name, long clubId)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult)
        {
            return new UnauthorizedResult();
        }

        OkObjectResult user = (OkObjectResult)r;
        if (user.Value == null)
        {
            return new UnauthorizedResult();
        }

        User u = (User)user.Value;
        if (u.RelatedTo.Id != clubId || u.Role != Role.Staff && u.Role != Role.Admin)
        {
            return new UnauthorizedResult();
        }

        using (MySQLController conn = new MySQLController())
        {
            conn.ExecuteQuery("INSERT INTO team (name, club_id) VALUES (@name, @clubId)", new Dictionary<string, object>
            {
                { "@name", name },
                { "@clubId", clubId }
            });
        }

        return new OkResult();
    }

    public ActionResult UpdateTeam(string token, long id, string name)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult)
        {
            return new UnauthorizedResult();
        }

        OkObjectResult user = (OkObjectResult)r;
        if (user.Value == null)
        {
            return new UnauthorizedResult();
        }

        User u = (User)user.Value;

        using (MySQLController conn = new MySQLController())
        {
            var result = conn.ExecuteQuery("SELECT club_id FROM team WHERE team_id = @id", new Dictionary<string, object>
            {
                { "@id", id }
            });

            if (result.Rows.Count == 0)
            {
                return new NotFoundResult();
            }

            DataRow row = result.Rows[0];
            if (u.RelatedTo.Id != Convert.ToInt64(row["club_id"]) || u.Role != Role.Staff && u.Role != Role.Admin)
            {
                return new UnauthorizedResult();
            }

            conn.ExecuteQuery("UPDATE team SET name = @name WHERE team_id = @id",
                new Dictionary<string, object>
                {
                    { "@name", name },
                    { "@id", id }
                });
        }

        return new OkResult();
    }

    public ActionResult DeleteTeam(string token, long id)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult)
        {
            return new UnauthorizedResult();
        }

        OkObjectResult user = (OkObjectResult)r;
        if (user.Value == null)
        {
            return new UnauthorizedResult();
        }

        User u = (User)user.Value;

        using (MySQLController conn = new MySQLController())
        {
            var result = conn.ExecuteQuery("SELECT * FROM team WHERE team_id = @id", new Dictionary<string, object>
            {
                { "@id", id }
            });

            if (result.Rows.Count == 0)
            {
                return new NotFoundResult();
            }

            DataRow row = result.Rows[0];

            if (u.RelatedTo.Id != Convert.ToInt64(row["club_id"]) || u.Role != Role.Staff && u.Role != Role.Admin)
            {
                return new UnauthorizedResult();
            }

            conn.ExecuteQuery("DELETE FROM team WHERE team_id = @id", new Dictionary<string, object>
            {
                { "@id", id }
            });
        }

        return new OkResult();
    }
}