using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.TeamModel;
using ScorePALServerService.Interfaces;

namespace ScorePALServerController.Controllers;

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

    [HttpPost("{id}")]
    public ActionResult<Team> GetTeam(string token, Team team)
    {
        return service.GetTeam(token, team);
    }

    [HttpPost("create")]
    public ActionResult CreateTeam([FromBody] string token, string name, long clubId)
    {
        return service.CreateTeam(token, name, clubId);
    }

    [Authorize]
    [HttpPut("update/{id}")]
    public ActionResult UpdateTeam([FromBody] long id, string name)
    {
        tokenService.CheckIfUserIsAdminStaffOrCoach(HttpContext.User);
        return service.UpdateTeam(HttpContext.User, id, name);
    }
}