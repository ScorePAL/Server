using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.MatchModel;

namespace ScorePALServerService.Interfaces;

public interface IMatchService
{
    /// <summary>
    /// Updates the score of a match.
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="match"></param>
    ActionResult UpdateMatchScore(ClaimsPrincipal claims, Match match);

    /// <summary>
    /// Get a match by its id.
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="match"></param>
    /// <returns>The match</returns>
    ActionResult<Match?> GetMatch(ClaimsPrincipal claims, Match match);

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
    /// Returns the upcoming matches of a club.
    /// </summary>
    /// <param name="club"></param>
    /// <returns></returns>
    ActionResult<Match[]> GetClubMatches(Club club);
}