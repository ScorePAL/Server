using Microsoft.AspNetCore.Mvc;
using Model.DAO.Interfaces;
using Model.Logic.MatchModel;
using ScorePALServer.Service.Interfaces;

namespace Service.Implementation;

public class MatchService : IMatchService
{
    private IMatchDao dao;

    public MatchService(IMatchDao dao)
    {
        this.dao = dao;
    }

    public ActionResult UpdateMatchScore(string token, long matchId, int scoreTeam1, int scoreTeam2)
    {
        return dao.UpdateMatchScore(token, matchId, scoreTeam1, scoreTeam2);
    }

    public ActionResult<Match?> GetMatch(string token, long matchId)
    {
        return dao.GetMatch(token, matchId);
    }

    public ActionResult<List<Match>> GetAllMatches(string token, long page, long limit)
    {
        return dao.GetAllMatches(token, page, limit);
    }

    public ActionResult<long> CreateMatch(string token, Match match)
    {
        return dao.CreateMatch(token, match);
    }

    public ActionResult<List<Match>> GetClubMatches(string token, long clubId)
    {
        return dao.GetClubMatches(token, clubId);
    }
}