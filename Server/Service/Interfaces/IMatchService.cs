using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.MatchModel;

namespace ScorePALServer.Service.Interfaces;

public interface IMatchService
{
    /// <summary>
    /// Updates the score of a match.
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="matchId">The match id</param>
    /// <param name="scoreTeam1">The new team1 score</param>
    /// <param name="scoreTeam2">The new team2 score</param>
    ActionResult UpdateMatchScore(ClaimsPrincipal claims, long matchId, int scoreTeam1, int scoreTeam2);

    /// <summary>
    /// Get a match by its id.
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="matchId">The match id</param>
    /// <returns>The match</returns>
    ActionResult<Match?> GetMatch(ClaimsPrincipal claims, long matchId);

    /// <summary>
    /// Get all matches by page.
    /// </summary>
    /// <param name="page">The page number</param>
    /// <param name="limit">The number of matches per page</param>
    /// <returns>All the matches of the page</returns>
    ActionResult<Match[]> GetAllMatches(long page, long limit);

    /// <summary>
    /// Create a match in the database.
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="match"></param>
    /// <returns></returns>
    ActionResult<long> CreateMatch(ClaimsPrincipal claims, Match match);

    /// <summary>
    /// Returns the uppcoming matches of a club.
    /// </summary>
    /// <param name="clubId">The club's id</param>
    /// <returns></returns>
    ActionResult<Match[]> GetClubMatches(ClaimsPrincipal claims, long clubId);
}