using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.MatchModel;
using ScorePALServerController.Events.Events;
using ScorePALServerController.SSE;
using ScorePALServerService.Interfaces;

namespace ScorePALServerController.Controllers;

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
    public async Task<IActionResult> UpdateScore(Match match)
    {
        var result = service.UpdateMatchScore(HttpContext.User, match);
        if (result is OkObjectResult)
        {
            var response = (OkObjectResult) result;
            var clubs = (List<long>) response.Value!;

            await eventPublisher.PublishEvent(new ScoreUpdatedEvent(match), clubs);
        }
        return result is OkObjectResult ? new OkResult() : result;
    }

    [Authorize]
    [HttpPost("{matchId}")]
    public ActionResult<Match?> GetMatch(Match match)
    {
        return service.GetMatch(HttpContext.User, match);
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
    public ActionResult<Match[]> GetClubMatches(Club club)
    {
        return service.GetClubMatches(club);
    }
}