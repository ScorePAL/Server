using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> UpdateScore(int matchId, int scoreTeam1, int scoreTeam2)
    {
        service.UpdateMatchScore(matchId, scoreTeam1, scoreTeam2);
        await webSocketManager.BroadcastMessageAsync("updateScore", new Dictionary<string, dynamic>
        {
            ["matchId"] = matchId,
            ["scoreTeam1"] = scoreTeam1,
            ["scoreTeam2"] = scoreTeam2
        });
        return new OkResult();
    }
}