using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.TeamModel;
using ScorePALServerService.Interfaces;

namespace ScorePALServerController.Controllers;

[Route("/api/team")]
public class TeamController
{
    private ITeamService service;

    public TeamController(ITeamService service)
    {
        this.service = service;
    }

    [HttpGet("all")]
    public ActionResult<List<Team>> GetTeams(string token, long page, long limit)
    {
        return service.GetTeams(token, page, limit);
    }

    [HttpGet("{id}")]
    public ActionResult<Team> GetTeam(string token, long id)
    {
        return service.GetTeam(token, id);
    }

    [HttpPost("create")]
    public ActionResult CreateTeam([FromBody] string token, string name, long clubId)
    {
        return service.CreateTeam(token, name, clubId);
    }

    [HttpPut("update/{id}")]
    public ActionResult UpdateTeam([FromBody] string token, long id, string name)
    {
        return service.UpdateTeam(token, id, name);
    }

    [HttpDelete("delete/{id}")]
    public ActionResult DeleteTeam(string token, long id)
    {
        return service.DeleteTeam(token, id);
    }
}