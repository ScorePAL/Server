using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Model.MatchModel;
using VamosVamosServer.Service.Interfaces;

namespace VamosVamosServer.Service.Implementation;

public class MatchService : IMatchService
{
    private IMatchDAO dao;

    public MatchService(IMatchDAO dao)
    {
        this.dao = dao;
    }

    public void UpdateMatchScore(string token, int matchId, int scoreTeam1, int scoreTeam2)
    {
        dao.UpdateMatchScore(token, matchId, scoreTeam1, scoreTeam2);
    }

    public Match GetMatch(string token, int matchId)
    {
        return dao.GetMatch(token, matchId);
    }

    public List<Match> GetAllMatches(string token, int page, int limit)
    {
        return dao.GetAllMatches(token, page, limit);
    }
}