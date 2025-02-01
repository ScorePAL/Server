using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.MatchHistoryModel;
using ScorePALServer.Model.MatchModel;
using ScorePALServer.Service.Interfaces;
using ScorePALServer.SSE;
using ScorePALServer.SSE.Events;

namespace ScorePALServer.Controllers;

[Route("api/match")]
public class MatchController
{
    private readonly IEventPublisher eventPublisher;
    private readonly IMatchService service;

    public MatchController(IEventPublisher eventPublisher, IMatchService matchService)
    {
        this.eventPublisher = eventPublisher;
        service = matchService;
    }

    [HttpPut("update-score/{matchId}")]
    public async Task<IActionResult> UpdateScore([FromBody] string token, long matchId, int scoreTeam1, int scoreTeam2)
    {
        var result = service.UpdateMatchScore(token, matchId, scoreTeam1, scoreTeam2);
        if (result is OkObjectResult)
        {
            var response = (OkObjectResult) result;
            var clubs = (List<long>) response.Value!;

            await eventPublisher.PublishEvent(new ScoreUpdatedEvent(matchId, scoreTeam1, scoreTeam2), clubs);
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