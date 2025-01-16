using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.Model.MatchModel;
using VamosVamosServer.Service.Interfaces;

namespace VamosVamosServer.Controllers;

[Route("api/match")]
public class MatchController
{
    private readonly WebSocketManager webSocketManager;
    private readonly IMatchService service;

    public MatchController(WebSocketManager webSocketManager, IMatchService service)
    {
        this.webSocketManager = webSocketManager;
        this.service = service;
    }

    [HttpPut("update-score/{matchId}")]
    public async Task<IActionResult> UpdateScore([FromBody] string token, long matchId, long scoreTeam1, long scoreTeam2)
    {
        var result = service.UpdateMatchScore(token, matchId, scoreTeam1, scoreTeam2);
        if (result is OkObjectResult)
        {
            var response = (OkObjectResult) result;
            var clubs = (List<long>) response.Value!;

            // TODO: Notify all users that are concerned by the match
        }
        return result is OkObjectResult ? new OkResult() : result;
    }

    [HttpGet("{matchId}")]
    public ActionResult<Match?> GetMatch([FromBody] string token, long matchId)
    {
        return service.GetMatch(token, matchId);
    }

    [HttpGet("all")]
    public ActionResult<List<Match>> GetAllMatches([FromBody] string token, long page = 1, long limit = 10)
    {
        return service.GetAllMatches(token, page, limit);
    }

    [HttpPost("create")]
    public ActionResult<long> CreateMatch([FromBody] string token, [FromBody] Match match)
    {
        return service.CreateMatch(token, match);
    }

    [HttpGet("club/{clubId}")]
    public ActionResult<List<Match>> GetClubMatches([FromBody] string token, long clubId)
    {
        return service.GetClubMatches(token, clubId);
    }
}