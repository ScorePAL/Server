using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.MatchModel;
using ScorePALServerController.Events.Events;
using ScorePALServerController.SSE;
using ScorePALServerService.Interfaces;

namespace ScorePALServerController.Controllers;

[Route("api/match")]
public class MatchController(IEventPublisher eventPublisher, IMatchService matchService) : ControllerBase
{
    [Authorize]
    [HttpPut("update-score/{matchId}")]
    public async Task<IActionResult> UpdateScore([FromBody] Match match)
    {
        var result = matchService.UpdateMatchScore(HttpContext.User, match);
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
    public ActionResult<Match?> GetMatch([FromBody] Match match)
    {
        return matchService.GetMatch(HttpContext.User, match);
    }

    [Authorize]
    [HttpGet("all")]
    public ActionResult<Match[]> GetAllMatches(long page = 1, long limit = 10)
    {
        return matchService.GetAllMatches(page, limit);
    }

    [Authorize]
    [HttpPost("create")]
    public ActionResult<long> CreateMatch([FromBody] Match match)
    {
        return matchService.CreateMatch(HttpContext.User, match);
    }

    [Authorize]
    [HttpGet("club/{clubId}")]
    public ActionResult<Match[]> GetClubMatches([FromBody] Club club)
    {
        return matchService.GetClubMatches(club);
    }
}