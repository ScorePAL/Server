using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.TeamModel;

namespace ScorePALServer.DAO.Interfaces;

public interface ITeamDAO
{
    /// <summary>
    /// Returns a list of teams
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="page">The page to look at</param>
    /// <param name="limit">The number of team per page</param>
    /// <returns>All the teams in the page</returns>
    public ActionResult<List<Team>> GetTeams(string token, long page, long limit);

    /// <summary>
    /// Returns a team by its id
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="id">The team id</param>
    /// <returns>The team</returns>
    public ActionResult<Team> GetTeam(string token, long id);

    /// <summary>
    /// Create a team with the given name and club id
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="name">The name of the team</param>
    /// <param name="clubId">The id of the club</param>
    /// <returns></returns>
    public ActionResult CreateTeam(string token, string name, long clubId);

    /// <summary>
    /// Update a team with the given id
    /// </summary>
    /// <param name="token">T</param>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public ActionResult UpdateTeam(string token, long id, string name);

    /// <summary>
    /// Delete a team with the given id
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <param name="id">The team id</param>
    /// <returns></returns>
    public ActionResult DeleteTeam(string token, long id);
}