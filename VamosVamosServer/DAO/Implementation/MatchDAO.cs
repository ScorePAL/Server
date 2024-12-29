using System.Data;
using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Model.ClubModel;
using VamosVamosServer.Model.MatchModel;
using VamosVamosServer.Model.Team;
using VamosVamosServer.Model.User;

namespace VamosVamosServer.DAO.Implementation;

public class MatchDAO : IMatchDAO
{
    public ActionResult UpdateMatchScore(string token, int matchId, int scoreTeam1, int scoreTeam2)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult)
        {
            return new UnauthorizedResult();
        }

        OkObjectResult user = (OkObjectResult)r;
        if (user.Value != null)
        {
            User u = (User)user.Value;

            using (MySQLController conn = new MySQLController())
            {
                var result = conn.ExecuteQuery(
                    "SELECT t1.club_id, t2.club_id FROM `match` m INNER JOIN team t1 on t1.team_id = m.team1_id INNER JOIN team t2 on t2.team_id = m.team2_id WHERE m.match_id = @matchId",
                    new Dictionary<string, object>
                    {
                        { "@matchId", matchId }
                    }
                );

                if (result.Rows.Count == 0)
                {
                    return new NotFoundObjectResult("Match not found.");
                }

                if (Convert.ToInt64(result.Rows[0]["t1.club_id"]) != u.RelatedTo.Id &&
                    Convert.ToInt64(result.Rows[0]["t2.club_id"]) != u.RelatedTo.Id &&
                    u.Role != Role.Admin)
                {
                    return new UnauthorizedResult();
                }

                conn.ExecuteQuery(
                    "UPDATE match SET score_team1 = @scoreTeam1, score_team2 = @scoreTeam2 WHERE id = @matchId",
                    new Dictionary<string, object>
                    {
                        { "@scoreTeam1", scoreTeam1 },
                        { "@scoreTeam2", scoreTeam2 },
                        { "@matchId", matchId }
                    }
                );
            }
        }

        return new OkResult();
    }

    public ActionResult<Match?> GetMatch(string token, int matchId)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult)
        {
            return new UnauthorizedResult();
        }


        Match? match = null;
        OkObjectResult user = (OkObjectResult) r;
        if (user.Value != null)
        {
            User u = (User)user.Value;
            using (MySQLController conn = new MySQLController())
            {
                var result = conn.ExecuteQuery(
                    "SELECT * FROM `match` m " +
                    "INNER JOIN team t1 on m.team1_id = t1.team_id " +
                    "INNER JOIN team t2 on m.team2_id = t2.team_id " +
                    "INNER JOIN club c1 on t1.club_id = c1.club_id " +
                    "INNER JOIN club c2 on t2.club_id = c2.club_id " +
                    "WHERE m.match_id = @matchId",
                    new Dictionary<string, object>
                    {
                        { "@matchId", matchId }
                    }
                );

                if (Convert.ToInt64(result.Rows[0]["t1.club_id"]) != u.RelatedTo.Id &&
                    Convert.ToInt64(result.Rows[0]["t2.club_id"]) != u.RelatedTo.Id &&
                    u.Role != Role.Admin)
                {
                    return new UnauthorizedResult();
                }

                if (result.Rows.Count == 0)
                {
                    return new NotFoundResult();
                }

                match = new Match
                {
                    Id = Convert.ToInt32(result.Rows[0]["id"]),
                    Team1 = new Team
                    {
                        Id = Convert.ToInt32(result.Rows[0]["team1_id"]),
                        Name = result.Rows[0]["t1.name"].ToString() ?? "",
                        Club = new Club
                        {
                            Id = Convert.ToInt32(result.Rows[0]["club1_id"]),
                            Name = result.Rows[0]["c1.name"].ToString() ?? ""
                        }
                    },
                    Team2 = new Team
                    {
                        Id = Convert.ToInt32(result.Rows[0]["team2_id"]),
                        Name = result.Rows[0]["t2.name"].ToString() ?? "",
                        Club = new Club
                        {
                            Id = Convert.ToInt32(result.Rows[0]["club2_id"]),
                            Name = result.Rows[0]["c2.name"].ToString() ?? ""
                        }
                    },
                    Date = Convert.ToDateTime(result.Rows[0]["date"]),
                    Address = result.Rows[0]["address"].ToString() ?? "",
                    Coach = result.Rows[0]["coach"].ToString() ?? "",
                    IsHome = Convert.ToBoolean(result.Rows[0]["is_home"]),
                    State = (MatchState)Convert.ToInt32(result.Rows[0]["match_state"]),
                    StartedTime = Convert.ToDateTime(result.Rows[0]["started_time"]),
                    Score1 = Convert.ToInt32(result.Rows[0]["score_team1"]),
                    Score2 = Convert.ToInt32(result.Rows[0]["score_team2"])
                };
            }
        }

        return new OkObjectResult(match);
    }

    public ActionResult<List<Match>> GetAllMatches(string token, int page, int limit)
    {
        UserDAO userDao = new UserDAO();
        ActionResult r = userDao.GetUserByToken(token);
        if (r is not OkObjectResult)
        {
            return new UnauthorizedObjectResult(new List<Match>());
        }


        List<Match> matches = new List<Match>();
        OkObjectResult user = (OkObjectResult)r;
        if (user.Value != null)
        {
            User u = (User)user.Value;
            using (MySQLController conn = new MySQLController())
            {
                var result = conn.ExecuteQuery(
                    "SELECT * FROM `match` m " +
                    "INNER JOIN team t1 on m.team1_id = t1.team_id " +
                    "INNER JOIN team t2 on m.team2_id = t2.team_id " +
                    "INNER JOIN club c1 on t1.club_id = c1.club_id " +
                    "INNER JOIN club c2 on t2.club_id = c2.club_id " +
                    "ORDER BY m.date DESC " +
                    "LIMIT @limit OFFSET @offset",
                    new Dictionary<string, object>
                    {
                        { "@limit", limit },
                        { "@offset", (page - 1) * limit }
                    }
                );

                foreach (DataRow row in result.Rows)
                {
                    Match match = new Match();
                    match.Id = Convert.ToInt32(row["id"]);
                    match.Team1 = new Team
                    {
                        Id = Convert.ToInt32(row["team1_id"]),
                        Name = row["t1.name"].ToString() ?? "",
                        Club = new Club
                        {
                            Id = Convert.ToInt32(row["club1_id"]),
                            Name = row["c1.name"].ToString() ?? ""
                        }
                    };

                    match.Team2 = new Team
                    {
                        Id = Convert.ToInt32(row["team2_id"]),
                        Name = row["t2.name"].ToString() ?? "",
                        Club = new Club
                        {
                            Id = Convert.ToInt32(row["club2_id"]),
                            Name = row["c2.name"].ToString() ?? ""
                        }
                    };

                    match.Date = Convert.ToDateTime(row["date"]);
                    match.Address = row["address"].ToString() ?? "";
                    match.Coach = row["coach"].ToString() ?? "";
                    match.IsHome = Convert.ToBoolean(row["is_home"]);
                    match.State = (MatchState)Convert.ToInt32(row["match_state"]);
                    match.StartedTime = Convert.ToDateTime(row["started_time"]);
                    match.Score1 = Convert.ToInt32(row["score_team1"]);
                    match.Score2 = Convert.ToInt32(row["score_team2"]);

                    matches.Add(match);
                }
            }
        }

        return new OkObjectResult(matches);
    }
}