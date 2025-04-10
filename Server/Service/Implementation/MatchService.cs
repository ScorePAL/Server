using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Model.MatchModel;
using ScorePALServer.Model.UserModel;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Service.Implementation;

public class MatchService : IMatchService
{
    private IMatchDAO dao;
    private ITokenService tokenService;

    public MatchService(IMatchDAO dao, ITokenService tokenService)
    {
        this.dao = dao;
    }

    public ActionResult UpdateMatchScore(ClaimsPrincipal claims, long matchId, int scoreTeam1, int scoreTeam2)
    {
        User user = tokenService.ExtractUser(claims);
        return dao.UpdateMatchScore(user, matchId, scoreTeam1, scoreTeam2);
    }

    public ActionResult<Match?> GetMatch(ClaimsPrincipal claims, long matchId)
    {
        User user = tokenService.ExtractUser(claims);
        return dao.GetMatch(user, matchId);
    }

    public ActionResult<Match[]> GetAllMatches(long page, long limit)
    {
        return dao.GetAllMatches(page, limit);
    }

    public ActionResult<long> CreateMatch(ClaimsPrincipal claims, Match match)
    {
        User user = tokenService.ExtractUser(claims);
        return dao.CreateMatch(user, match);
    }

    public ActionResult<Match[]> GetClubMatches(ClaimsPrincipal claims, long clubId)
    {
        return dao.GetClubMatches(clubId);
    }
}