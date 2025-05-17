using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.Logic.MatchModel;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Exceptions.Match;
using ScorePALServerModel.Exceptions.Team;
using ScorePALServerModel.Exceptions.User;
using ScorePALServerModel.Logic.ClubModel;

namespace ScorePALServerModel.DAO.Interfaces;

public interface IMatchDao
{
    /// <summary>
    /// Updates the score of a match.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="match"></param>
    /// <exception cref="MatchNotFoundException"></exception>
    ActionResult UpdateMatchScore(User user, Match match);

    /// <summary>
    /// Get a match by its id.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="match">The match id</param>
    /// <exception cref="InvalidPermissionsException"></exception>
    /// <exception cref="MatchNotFoundException"></exception>
    /// <returns>The match</returns>
    ActionResult<Match?> GetMatch(User user, Match match);

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
    /// <param name="user"></param>
    /// <param name="match"></param>
    /// <exception cref="TeamNotFoundException"></exception>
    /// <exception cref="InvalidPermissionsException"></exception>
    /// <returns></returns>
    ActionResult<long> CreateMatch(User user, Match match);

    /// <summary>
    /// Returns the uppcoming matches of a club.
    /// </summary>
    /// <param name="clubId">The club's id</param>
    /// <returns></returns>
    ActionResult<Match[]> GetClubMatches(Club clubId);
}