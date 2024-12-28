using VamosVamosServer.Model.MatchModel;

namespace VamosVamosServer.Service.Interfaces;

public interface IMatchService
{
    /// <summary>
    /// Updates the score of a match.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="matchId">The match id</param>
    /// <param name="scoreTeam1">The new team1 score</param>
    /// <param name="scoreTeam2">The new team2 score</param>
    void UpdateMatchScore(string token, int matchId, int scoreTeam1, int scoreTeam2);

    /// <summary>
    /// Get a match by its id.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="matchId">The match id</param>
    /// <returns>The match</returns>
    Match GetMatch(string token, int matchId);

    /// <summary>
    /// Get all matches by page.
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="page">The page number</param>
    /// <param name="limit">The number of matches per page</param>
    /// <returns>All the matches of the page</returns>
    List<Match> GetAllMatches(string token, int page, int limit);
}