using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.TeamModel;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Controllers;

[Route("/api/team")]
public class TeamController : ControllerBase
{
    private ITeamService service;
    private ITokenService tokenService;

    public TeamController(ITeamService service, ITokenService tokenService)
    {
        this.service = service;
        this.tokenService = tokenService;
    }

    [Authorize]
    [HttpGet("all")]
    public ActionResult<Team[]> GetTeams(long page, long limit)
    {
        tokenService.CheckIfUserIsAdmin(HttpContext.User);
        return service.GetTeams(page, limit);
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<Team> GetTeam(long id)
    {
        return service.GetTeam(id);
    }

    [Authorize]
    [HttpPut("update/{id}")]
    public ActionResult UpdateTeam([FromBody] long id, string name)
    {
        tokenService.CheckIfUserIsAdminStaffOrCoach(HttpContext.User);
        return service.UpdateTeam(HttpContext.User, id, name);
    }
}