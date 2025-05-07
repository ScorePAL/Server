using Microsoft.AspNetCore.Mvc;
using Model.Logic.ClubModel;
using Model.Logic.MatchModel;

namespace Model.DAO.Interfaces;

public interface IMatchDao
{
    /// <summary>
    /// Updates the score of a match.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="matchId">The match id</param>
    ActionResult UpdateMatchScore(string token, Match matchId);

    /// <summary>
    /// Get a match by its id.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="match"></param>
    /// <returns>The match</returns>
    ActionResult<Match?> GetMatch(string token, Match match);

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
    /// <param name="token"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    ActionResult<long> CreateMatch(string token, Match match);

    /// <summary>
    /// Returns the uppcoming matches of a club.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="clubId">The club's id</param>
    /// <returns></returns>
    ActionResult<List<Match>> GetClubMatches(string token, Club clubId);
}