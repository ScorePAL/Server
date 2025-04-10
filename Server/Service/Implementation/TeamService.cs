using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Model.Team;
using ScorePALServer.Model.UserModel;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Service.Implementation;

public class TeamService : ITeamService
{
    private ITeamDAO dao;
    private ITokenService tokenService;

    public TeamService(ITeamDAO dao, ITokenService tokenService)
    {
        this.dao = dao;
        this.tokenService = tokenService;
    }

    public ActionResult<Team[]> GetTeams(long page, long limit)
    {
        return dao.GetTeams(page, limit);
    }

    public ActionResult<Team> GetTeam(long id)
    {
        return dao.GetTeam(id);
    }

    public ActionResult UpdateTeam(ClaimsPrincipal claims, long id, string name)
    {
        User user = tokenService.ExtractUser(claims);
        return dao.UpdateTeam(user, id, name);
    }
}