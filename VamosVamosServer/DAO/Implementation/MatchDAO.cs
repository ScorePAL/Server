using MySqlConnector;
using VamosVamosServer.DAO.Interfaces;

namespace VamosVamosServer.DAO.Implementation;

public class MatchDAO : IMatchDAO
{

    public void UpdateMatchScore(int matchId, int scoreTeam1, int scoreTeam2)
    {
        using (MySQLController conn = new MySQLController())
        {
            ///conn.ExecuteQuery("UPDATE match SET score_team1 = @scoreTeam1, score_team2 = @scoreTeam2 WHERE id = @matchId",
               /// new Dictionary<string, object>
                ///{
                   /// {"@scoreTeam1", scoreTeam1},
                    ///{"@scoreTeam2", scoreTeam2},
                    ///{"@matchId", matchId}
                ///});
                ///
            Console.WriteLine("Match updated");
        }
    }
}