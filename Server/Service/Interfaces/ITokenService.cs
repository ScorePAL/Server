using System.Security.Claims;
using ScorePALServer.Model.UserModel;

namespace ScorePALServer.Service.Interfaces;

public interface ITokenService
{
    public string Create(User user);
    string CreateRefresh(User user);
    User ExtractUser(ClaimsPrincipal claims);
    void CheckIfUserIsAdminStaffOrCoach(ClaimsPrincipal httpContextUser);
    void CheckIfUserIsAdmin(ClaimsPrincipal httpContextUser);
    void CheckIfUserIsAdminOrCoach(ClaimsPrincipal httpContextUser);
}