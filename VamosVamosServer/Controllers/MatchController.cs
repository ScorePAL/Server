using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.Model.MatchModel;
using VamosVamosServer.Service.Interfaces;
using WebSocketManager = VamosVamosServer.WebSockets.WebSocketManager;

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

    [HttpPost("update-score")]
    public async Task<IActionResult> UpdateScore(string token, long matchId, long scoreTeam1, long scoreTeam2)
    {
        var result = service.UpdateMatchScore(token, matchId, scoreTeam1, scoreTeam2);
        if (result is OkObjectResult)
        {
            var response = (OkObjectResult) result;
            var clubs = (List<int>) response.Value!;

            foreach (var club in clubs)
            {
                await webSocketManager.BroadcastMessageAsync(club, "updateScore", new Dictionary<string, dynamic>
                {
                    ["matchId"] = matchId,
                    ["scoreTeam1"] = scoreTeam1,
                    ["scoreTeam2"] = scoreTeam2
                });
            }


        }
        return result;
    }

    [HttpGet("{matchId}")]
    public ActionResult<Match?> GetMatch(string token, long matchId)
    {
        return service.GetMatch(token, matchId);
    }

    [HttpGet("all")]
    public ActionResult<List<Match>> GetAllMatches(string token, long page = 1, long limit = 10)
    {
        return service.GetAllMatches(token, page, limit);
    }

    [HttpPost("create")]
    public ActionResult<long> CreateMatch(string token, [FromBody] Match match)
    {
        return service.CreateMatch(token, match);
    }
}