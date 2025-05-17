using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.TeamModel;

namespace ScorePALServerService.Interfaces;

public interface ITeamService
{
    /// <summary>
    /// Returns a list of teams
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="page">The page to look at</param>
    /// <param name="limit">The number of team per page</param>
    /// <returns>All the teams in the page</returns>
    public Team[] GetTeams(ClaimsPrincipal claims, long page, long limit);

    /// <summary>
    /// Returns a team by its id
    /// </summary>
    /// <param name="id">The team id</param>
    /// <returns>The team</returns>
    public Team GetTeam(Team id);

    /// <summary>
    /// Create a team with the given name and club id
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="name"></param>
    /// <param name="club"></param>
    /// <returns></returns>
    public Team CreateTeam(ClaimsPrincipal claims, string name, Club club);

    /// <summary>
    /// Update a team with the given id
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="team"></param>
    /// <returns></returns>
    public Team UpdateTeam(ClaimsPrincipal claims, Team team);
}