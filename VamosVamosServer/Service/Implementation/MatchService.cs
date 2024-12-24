using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Service.Interfaces;

namespace VamosVamosServer.Service.Implementation;

public class MatchService : IMatchService
{
    private IMatchDAO dao;

    public MatchService(IMatchDAO dao)
    {
        this.dao = dao;
    }

    public void UpdateMatchScore(int matchId, int scoreTeam1, int scoreTeam2)
    {
        dao.UpdateMatchScore(matchId, scoreTeam1, scoreTeam2);
    }
}