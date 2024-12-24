using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.Service.Interfaces;
using WebSocketManager = VamosVamosServer.WebSockets.WebSocketManager;

namespace VamosVamosServer.Controllers;

[Route("api/match")]
public class MatchController
{
    private readonly WebSocketManager _webSocketManager;
    private readonly IMatchService _service;

    public MatchController(WebSocketManager webSocketManager, IMatchService service)
    {
        _webSocketManager = webSocketManager;
        _service = service;
    }

    [HttpPost("update-score")]
    public async Task<IActionResult> UpdateScore(int matchId, int scoreTeam1, int scoreTeam2)
    {
        _service.UpdateMatchScore(matchId, scoreTeam1, scoreTeam2);
        await _webSocketManager.BroadcastMessageAsync($"Match {matchId} updated with score {scoreTeam1} - {scoreTeam2}");
        return new OkResult();
    }
}