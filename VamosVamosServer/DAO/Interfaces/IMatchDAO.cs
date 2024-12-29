using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.Model.MatchModel;

namespace VamosVamosServer.DAO.Interfaces;

public interface IMatchDAO
{
    /// <summary>
    /// Updates the score of a match.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="matchId">The match id</param>
    /// <param name="scoreTeam1">The new team1 score</param>
    /// <param name="scoreTeam2">The new team2 score</param>
    ActionResult UpdateMatchScore(string token, long matchId, long scoreTeam1, long scoreTeam2);

    /// <summary>
    /// Get a match by its id.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="matchId">The match id</param>
    /// <returns>The match</returns>
    ActionResult<Match?> GetMatch(string token, long matchId);

    /// <summary>
    /// Get all matches by page.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="page">The page number</param>
    /// <param name="limit">The number of matches per page</param>
    /// <returns>All the matches of the page</returns>
    ActionResult<List<Match>> GetAllMatches(string token, long page, long limit);

    /// <summary>
    /// Create a match in the database.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="match">The match to insert into the database</param>
    /// <returns></returns>
    ActionResult<long> CreateMatch(string token, Match match);
}