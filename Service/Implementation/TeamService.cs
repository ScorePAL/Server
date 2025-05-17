using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServerModel.DAO.Interfaces;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Logic.ClubModel;
using ScorePALServerModel.Logic.TeamModel;
using ScorePALServerService.Interfaces;

namespace ScorePALServerService.Implementation;

public class TeamService(ITeamDAO dao, ITokenService tokenService) : ITeamService
{
    public Team[] GetTeams(ClaimsPrincipal claims, long page, long limit)
    {
        tokenService.CheckIfUserIsAdmin(claims);
        return dao.GetTeams(page, limit);
    }


    public Team GetTeam(Team team)
    {
        return dao.GetTeam(team);
    }

    public Team CreateTeam(ClaimsPrincipal claims, string name, Club club)
    {
        tokenService.CheckIfUserIsAdminStaffOrCoach(claims);
        return dao.CreateTeam(name, club);
    }

    public Team UpdateTeam(ClaimsPrincipal claims, Team team)
    {
        User user = tokenService.ExtractUser(claims);
        tokenService.CheckIfUserIsAdminStaffOrCoach(claims);
        return dao.UpdateTeam(user, team);
    }
}