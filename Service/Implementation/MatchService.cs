using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.MatchModel;
using ScorePALServerService.Interfaces;

namespace ScorePALServerService.Implementation;

public class MatchService(IMatchDao dao, ITokenService tokenService) : IMatchService
{
    public ActionResult UpdateMatchScore(ClaimsPrincipal claims, Match match)
    {
        User user = tokenService.ExtractUser(claims);
        return dao.UpdateMatchScore(user, match);
    }

    public ActionResult<Match?> GetMatch(ClaimsPrincipal claims, Match match)
    {
        User user = tokenService.ExtractUser(claims);
        return dao.GetMatch(user, match);
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

    public ActionResult<Match[]> GetClubMatches(Club club)
    {
        return dao.GetClubMatches(club);
    }
}