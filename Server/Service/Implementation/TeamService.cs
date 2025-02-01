using Microsoft.AspNetCore.Mvc;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Model.Team;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Service.Implementation;

public class TeamService : ITeamService
{
    private ITeamDAO dao;

    public TeamService(ITeamDAO dao)
    {
        this.dao = dao;
    }

    public ActionResult<List<Team>> GetTeams(string token, long page, long limit)
    {
        return dao.GetTeams(token, page, limit);
    }

    public ActionResult<Team> GetTeam(string token, long id)
    {
        return dao.GetTeam(token, id);
    }

    public ActionResult CreateTeam(string token, string name, long clubId)
    {
        return dao.CreateTeam(token, name, clubId);
    }

    public ActionResult UpdateTeam(string token, long id, string name)
    {
        return dao.UpdateTeam(token, id, name);
    }

    public ActionResult DeleteTeam(string token, long id)
    {
        return dao.DeleteTeam(token, id);
    }
}