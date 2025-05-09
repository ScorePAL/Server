using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.TeamModel;
using ScorePALServer.Model.UserModel;

namespace ScorePALServerModel.DAO.Interfaces;

public interface ITeamDAO
{
    /// <summary>
    /// Returns a list of teams
    /// </summary>
    /// <param name="page">The page to look at</param>
    /// <param name="limit">The number of team per page</param>
    /// <returns>All the teams in the page</returns>
    public ActionResult<Team[]> GetTeams(long page, long limit);

    /// <summary>
    /// Returns a team by its id
    /// </summary>
    /// <param name="id">The team id</param>
    /// <returns>The team</returns>
    public ActionResult<Team> GetTeam(long id);

    /// <summary>
    /// Update a team with the given id
    /// </summary>
    /// <param name="user"></param>
    /// <param name="id"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public ActionResult UpdateTeam(User user, long id, string name);
}