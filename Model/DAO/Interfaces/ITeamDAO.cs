using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.TeamModel;

namespace ScorePALServerModel.DAO.Interfaces;

public interface ITeamDAO
{
    /// <summary>
    /// Returns a list of teams
    /// </summary>
    /// <param name="page">The page to look at</param>
    /// <param name="limit">The number of team per page</param>
    /// <returns>All the teams in the page</returns>
    public Team[] GetTeams(long page, long limit);

    /// <summary>
    /// Returns a team by its id
    /// </summary>
    /// <param name="team">The team id</param>
    /// <returns>The team</returns>
    public Team GetTeam(Team team);

    /// <summary>
    /// Create a team with the given name and club id
    /// </summary>
    /// <param name="name">The name of the team</param>
    /// <param name="club"></param>
    /// <returns></returns>
    public Team CreateTeam(string name, Club club);

    /// <summary>
    /// Update a team with the given id
    /// </summary>
    /// <param name="user"></param>
    /// <param name="team"></param>
    /// <returns></returns>
    public Team UpdateTeam(User user, Team team);
}