using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.Model.Team;
using VamosVamosServer.Service.Interfaces;

namespace VamosVamosServer.Controllers;

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
    public ActionResult CreateTeam(string token, string name, long clubId)
    {
        return service.CreateTeam(token, name, clubId);
    }

    [HttpPut("update")]
    public ActionResult UpdateTeam(string token, long id, string name)
    {
        return service.UpdateTeam(token, id, name);
    }

    [HttpDelete("delete/{id}")]
    public ActionResult DeleteTeam(string token, long id)
    {
        return service.DeleteTeam(token, id);
    }
}