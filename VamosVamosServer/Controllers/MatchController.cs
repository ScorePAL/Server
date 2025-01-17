using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.Model.MatchModel;
using VamosVamosServer.Service.Interfaces;
using VamosVamosServer.SSE;

namespace VamosVamosServer.Controllers;

[Route("api/match")]
public class MatchController
{
    private readonly SSEController sseController;
    private readonly IMatchService service;

    public MatchController(SSEController sseController, IMatchService service)
    {
        this.sseController = sseController;
        this.service = service;
    }

    [HttpPut("update-score/{matchId}")]
    public async Task<IActionResult> UpdateScore([FromBody] string token, long matchId, int scoreTeam1, int scoreTeam2)
    {
        var result = service.UpdateMatchScore(token, matchId, scoreTeam1, scoreTeam2);
        if (result is OkObjectResult)
        {
            var response = (OkObjectResult) result;
            var clubs = (List<long>) response.Value!;

            foreach (var clubId in clubs)
            {
                await sseController.SendMessage($"Match {matchId} updated", clubId);
            }
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