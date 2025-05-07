using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.MatchModel;
using ScorePALServer.Service.Interfaces;

namespace Service.Implementation;

public class MatchService : IMatchService
{
    private IMatchDao dao;

    public MatchService(IMatchDao dao)
    {
        this.dao = dao;
    }

    public ActionResult UpdateMatchScore(string token, Match matchId)
    {
        return dao.UpdateMatchScore(token, matchId);
    }

    public ActionResult<Match?> GetMatch(string token, Match match)
    {
        return dao.GetMatch(token, match);
    }

    public ActionResult<List<Match>> GetAllMatches(string token, long page, long limit)
    {
        return dao.GetAllMatches(token, page, limit);
    }

    public ActionResult<long> CreateMatch(string token, Match match)
    {
        return dao.CreateMatch(token, match);
    }

    public ActionResult<List<Match>> GetClubMatches(string token, Club club)
    {
        return dao.GetClubMatches(token, club);
    }
}