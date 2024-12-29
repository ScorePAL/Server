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
    public async Task<IActionResult> UpdateScore(string token, int matchId, int scoreTeam1, int scoreTeam2)
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

    [HttpGet]
    public ActionResult<Match?> GetMatch(string token, int matchId)
    {
        return service.GetMatch(token, matchId);
    }

    [HttpGet("all")]
    public ActionResult<List<Match>> GetAllMatches(string token, int page = 1, int limit = 10)
    {
        return service.GetAllMatches(token, page, limit);
    }
}