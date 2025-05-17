using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.TeamModel;
using ScorePALServerService.Interfaces;

namespace ScorePALServerController.Controllers;

[Route("/api/team")]
public class TeamController(ITeamService service, ITokenService tokenService) : ControllerBase
{
    [Authorize]
    [HttpGet("all")]
    public Team[] GetTeams(long page, long limit)
    {
        return service.GetTeams(HttpContext.User, page, limit);
    }

    [Authorize]
    [HttpPost("{id}")]
    public Team GetTeam([FromBody] Team team)
    {
        return service.GetTeam(team);
    }

    [HttpPost("create")]
    public Team CreateTeam(string name, [FromBody] Club club)
    {
        return service.CreateTeam(HttpContext.User, name, club);
    }

    [Authorize]
    [HttpPut("update/{id}")]
    public Team UpdateTeam([FromBody] Team team)
    {
        return service.UpdateTeam(HttpContext.User, team);
    }
}