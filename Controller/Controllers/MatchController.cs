using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.MatchModel;
using ScorePALServer.Service.Interfaces;
using ScorePALServerController.Events.Events;
using ScorePALServerController.SSE;

namespace ScorePALServerController.Controllers;

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
    public async Task<IActionResult> UpdateScore([FromBody] string token, Match match)
    {
        var result = service.UpdateMatchScore(token, match);
        if (result is OkObjectResult)
        {
            var response = (OkObjectResult) result;
            var clubs = (List<long>) response.Value!;

            await eventPublisher.PublishEvent(new ScoreUpdatedEvent(match), clubs);
        }
        return result is OkObjectResult ? new OkResult() : result;
    }

    [HttpPost("{matchId}")]
    public ActionResult<Match?> GetMatch(string token, Match match)
    {
        return service.GetMatch(token, match);
    }

    [HttpGet("all")]
    public ActionResult<List<Match>> GetAllMatches(string token, long page = 1, long limit = 10)
    {
        return service.GetAllMatches(token, page, limit);
    }

    [HttpPost("create")]
    public ActionResult<long> CreateMatch([FromBody] string token, Match match)
    {
        return service.CreateMatch(token, match);
    }

    [HttpPost("club/{clubId}")]
    public ActionResult<List<Match>> GetClubMatches(string token, Club club)
    {
        return service.GetClubMatches(token, club);
    }
}