using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.MatchHistoryModel;
using ScorePALServer.Model.MatchModel;
using ScorePALServer.Service.Interfaces;
using ScorePALServer.SSE;
using ScorePALServer.SSE.Events;

namespace ScorePALServer.Controllers;

[Route("api/match")]
public class MatchController : ControllerBase
{
    private readonly IEventPublisher eventPublisher;
    private readonly IMatchService service;

    public MatchController(IEventPublisher eventPublisher, IMatchService matchService)
    {
        this.eventPublisher = eventPublisher;
        service = matchService;
    }

    [Authorize]
    [HttpPut("update-score/{matchId}")]
    public async Task<IActionResult> UpdateScore(long matchId, int scoreTeam1, int scoreTeam2)
    {
        var result = service.UpdateMatchScore(HttpContext.User, matchId, scoreTeam1, scoreTeam2);
        if (result is OkObjectResult)
        {
            var response = (OkObjectResult) result;
            var clubs = (List<long>) response.Value!;

            await eventPublisher.PublishEvent(new ScoreUpdatedEvent(matchId, scoreTeam1, scoreTeam2), clubs);
        }
        return result is OkObjectResult ? new OkResult() : result;
    }

    [Authorize]
    [HttpGet("{matchId}")]
    public ActionResult<Match?> GetMatch(long matchId)
    {
        return service.GetMatch(HttpContext.User, matchId);
    }

    [Authorize]
    [HttpGet("all")]
    public ActionResult<Match[]> GetAllMatches(long page = 1, long limit = 10)
    {
        return service.GetAllMatches(page, limit);
    }

    [Authorize]
    [HttpPost("create")]
    public ActionResult<long> CreateMatch([FromBody] Match match)
    {
        return service.CreateMatch(HttpContext.User, match);
    }

    [Authorize]
    [HttpGet("club/{clubId}")]
    public ActionResult<Match[]> GetClubMatches(long clubId)
    {
        return service.GetClubMatches(HttpContext.User, clubId);
    }
}