using System.Data;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Exceptions.Match;
using ScorePALServer.Exceptions.Team;
using ScorePALServer.Exceptions.User;
using ScorePALServer.Model.ClubModel;
using ScorePALServer.Model.MatchModel;
using ScorePALServer.Model.Team;
using ScorePALServer.Model.UserModel;

namespace ScorePALServer.DAO.Implementation;

public class MatchDAO : IMatchDAO
{
    public ActionResult UpdateMatchScore(User user, long matchId, int scoreTeam1, int scoreTeam2)
    {
        OkObjectResult clubs;

        using MySqlController conn = new();
        var result = conn.ExecuteQuery(
            "SELECT t1.club_id as 't1.club_id', t2.club_id as 't2.club_id' " +
            "FROM `matches` m " +
            "INNER JOIN teams t1 on t1.team_id = m.team1_id " +
            "INNER JOIN teams t2 on t2.team_id = m.team2_id " +
            "WHERE m.match_id = @matchId",
            new Dictionary<string, object>
            {
                { "@matchId", matchId }
            }
        );

        if (result.Rows.Count == 0)
        {
            throw new MatchNotFoundException(matchId);
        }

        long team1ClubId = Convert.ToInt64(result.Rows[0]["t1.club_id"]);
        long team2ClubId = Convert.ToInt64(result.Rows[0]["t2.club_id"]);

        if (team1ClubId != user.RelatedTo.Id &&
            team2ClubId != user.RelatedTo.Id &&
            user.Role != Role.Admin)
        {
            return new UnauthorizedResult();
        }

        conn.ExecuteQuery(
            "UPDATE `matches` SET score_team1 = @scoreTeam1, score_team2 = @scoreTeam2 WHERE match_id = @matchId",
            new Dictionary<string, object>
            {
                { "@scoreTeam1", scoreTeam1 },
                { "@scoreTeam2", scoreTeam2 },
                { "@matchId", matchId }
            }
        );

        clubs = new OkObjectResult(new List<long>
        {
            team1ClubId,
            team2ClubId
        });

        return clubs;
    }

    public ActionResult<Match?> GetMatch(User user, long matchId)
    {
        Match? match = null;
        using MySqlController conn = new();
        var result = conn.ExecuteQuery(
            "SELECT * FROM `matches` m " +
            "INNER JOIN teams t1 on m.team1_id = t1.team_id " +
            "INNER JOIN teams t2 on m.team2_id = t2.team_id " +
            "INNER JOIN clubs c1 on t1.club_id = c1.club_id " +
            "INNER JOIN clubs c2 on t2.club_id = c2.club_id " +
            "WHERE m.match_id = @matchId",
            new Dictionary<string, object>
            {
                { "@matchId", matchId }
            }
        );

        if (Convert.ToInt64(result.Rows[0]["t1.club_id"]) != user.RelatedTo.Id &&
            Convert.ToInt64(result.Rows[0]["t2.club_id"]) != user.RelatedTo.Id &&
            user.Role != Role.Admin)
        {
            throw new InvalidPermissionsException("Can't get match for this team");
        }

        if (result.Rows.Count == 0)
        {
            throw new MatchNotFoundException(matchId);
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
            State = (MatchState)Convert.ToInt32(result.Rows[0]["match_state"]),
            StartedTime = Convert.ToDateTime(result.Rows[0]["started_time"]),
            Score1 = Convert.ToInt32(result.Rows[0]["score_team1"]),
            Score2 = Convert.ToInt32(result.Rows[0]["score_team2"])
        };


        return new OkObjectResult(match);
    }

    public ActionResult<Match[]> GetAllMatches(long page, long limit)
    {
        List<Match> matches = [];
        using MySqlController conn = new();
        var result = conn.ExecuteQuery(
            "SELECT * FROM matches m " +
            "INNER JOIN teams t1 on m.team1_id = t1.team_id " +
            "INNER JOIN teams t2 on m.team2_id = t2.team_id " +
            "INNER JOIN clubs c1 on t1.club_id = c1.club_id " +
            "INNER JOIN clubs c2 on t2.club_id = c2.club_id " +
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
            Match match = new Match
            {
                Id = Convert.ToInt32(row["id"]),
                Team1 = new Team
                {
                    Id = Convert.ToInt32(row["team1_id"]),
                    Name = row["t1.name"].ToString() ?? "",
                    Club = new Club
                    {
                        Id = Convert.ToInt32(row["club1_id"]),
                        Name = row["c1.name"].ToString() ?? ""
                    }
                },
                Team2 = new Team
                {
                    Id = Convert.ToInt32(row["team2_id"]),
                    Name = row["t2.name"].ToString() ?? "",
                    Club = new Club
                    {
                        Id = Convert.ToInt32(row["club2_id"]),
                        Name = row["c2.name"].ToString() ?? ""
                    }
                },
                Date = Convert.ToDateTime(row["date"]),
                Address = row["address"].ToString() ?? "",
                Coach = row["coach"].ToString() ?? "",
                State = (MatchState)Convert.ToInt32(row["match_state"]),
                StartedTime = Convert.ToDateTime(row["started_time"]),
                Score1 = Convert.ToInt32(row["score_team1"]),
                Score2 = Convert.ToInt32(row["score_team2"])
            };

            matches.Add(match);
        }

        return new OkObjectResult(matches.ToArray());
    }

    public ActionResult<long> CreateMatch(User user, Match match)
    {
        long matchId;
        using (MySqlController conn = new MySqlController())
        {
            bool canCreate = false;

            var result = conn.ExecuteQuery(
                "SELECT * FROM teams WHERE team_id = @teamId",
                new Dictionary<string, object>
                {
                    { "@teamId", match.Team1.Id }
                }
            );

            if (result.Rows.Count == 0)
            {
                throw new TeamNotFoundException(match.Team1.Id);
            }

            if (result.Rows[0]["club_id"].ToString() == user.RelatedTo.Id.ToString() || user.Role == Role.Admin)
            {
                canCreate = true;
            }

            result = conn.ExecuteQuery(
                "SELECT * FROM teams WHERE team_id = @teamId",
                new Dictionary<string, object>
                {
                    { "@teamId", match.Team2.Id }
                }
            );

            if (result.Rows.Count == 0)
            {
                throw new TeamNotFoundException(match.Team2.Id);
            }

            if (result.Rows[0]["club_id"].ToString() == user.RelatedTo.Id.ToString() || user.Role == Role.Admin)
            {
                canCreate = true;
            }

            if (!canCreate)
            {
                throw new InvalidPermissionsException("Can't create match for this team");
            }

            matchId = conn.ExecuteInsert(
                "INSERT INTO `matches` " +
                "(team1_id, team2_id, date, address, coach, match_state, started_time, score_team1, score_team2) " +
                "VALUES (@team1Id, @team2Id, @date, @address, @coach, @matchState, @startedTime, @scoreTeam1, @scoreTeam2)",
                new Dictionary<string, object>
                {
                    { "@team1Id", match.Team1.Id },
                    { "@team2Id", match.Team2.Id },
                    { "@date", match.Date },
                    { "@address", match.Address },
                    { "@coach", match.Coach },
                    { "@matchState", (int)match.State },
                    { "@startedTime", match.StartedTime },
                    { "@scoreTeam1", match.Score1 },
                    { "@scoreTeam2", match.Score2 }
                }
            );
        }

        return new OkObjectResult(matchId);
    }

    public ActionResult<Match[]> GetClubMatches(long clubId)
    {
        List<Match> matches = new List<Match>();

        using MySqlController conn = new MySqlController();
        var result = conn.ExecuteQuery(
            "SELECT match_id, team1_id, team2_id, date, address, coach, match_state, started_time, score_team1, score_team2, t1.name, t1.club_id, t2.name, t2.club_id, c1.name, c1.logo_url, c2.name, c2.logo_url FROM `matches` m " +
            "INNER JOIN teams t1 on m.team1_id = t1.team_id " +
            "INNER JOIN teams t2 on m.team2_id = t2.team_id " +
            "INNER JOIN clubs c1 on t1.club_id = c1.club_id " +
            "INNER JOIN clubs c2 on t2.club_id = c2.club_id " +
            "WHERE t1.club_id = @clubId OR t2.club_id = @clubId " +
            "ORDER BY m.date DESC",
            new Dictionary<string, object>
            {
                { "@clubId", clubId }
            }
        );

        foreach (DataRow row in result.Rows)
        {
            Match match = new Match
            {
                Id = Convert.ToInt32(row["id"]),
                Team1 = new Team
                {
                    Id = Convert.ToInt32(row["team1_id"]),
                    Name = row["t1.name"].ToString() ?? "",
                    Club = new Club
                    {
                        Id = Convert.ToInt32(row["club1_id"]),
                        Name = row["c1.name"].ToString() ?? ""
                    }
                },
                Team2 = new Team
                {
                    Id = Convert.ToInt32(row["team2_id"]),
                    Name = row["t2.name"].ToString() ?? "",
                    Club = new Club
                    {
                        Id = Convert.ToInt32(row["club2_id"]),
                        Name = row["c2.name"].ToString() ?? ""
                    }
                },
                Date = Convert.ToDateTime(row["date"]),
                Address = row["address"].ToString() ?? "",
                Coach = row["coach"].ToString() ?? "",
                State = (MatchState)Convert.ToInt32(row["match_state"]),
                StartedTime = Convert.ToDateTime(row["started_time"]),
                Score1 = Convert.ToInt32(row["score_team1"]),
                Score2 = Convert.ToInt32(row["score_team2"])
            };

            matches.Add(match);
        }

        return new OkObjectResult(matches);
    }
}