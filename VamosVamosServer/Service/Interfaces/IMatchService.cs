using VamosVamosServer.DAO.Interfaces;

namespace VamosVamosServer.Service.Interfaces;

public interface IMatchService
{
    /// <summary>
    /// Updates the score of a match.
    /// </summary>
    /// <param name="matchId">The match id</param>
    /// <param name="scoreTeam1">The new team1 score</param>
    /// <param name="scoreTeam2">The new team2 score</param>
    void UpdateMatchScore(int matchId, int scoreTeam1, int scoreTeam2);
}