using System.Security.Claims;
using ScorePALServer.Model.UserModel;

namespace ScorePALServerService.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// Create a token based on the given user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public string Create(User user);

    /// <summary>
    /// Create a refresh token based on the given user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string CreateRefresh(User user);

    /// <summary>
    /// Create a user based on a token
    /// </summary>
    /// <param name="claims">The claims of the tokens</param>
    /// <returns></returns>
    User ExtractUser(ClaimsPrincipal claims);

    /// <summary>
    /// Check if a token has the required permissions (Role.Admin, Role.Coach, Role.Staff)
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">If the token don't have the required permissions</exception>
    /// <param name="claims">The claims of the tokens</param>
    void CheckIfUserIsAdminStaffOrCoach(ClaimsPrincipal claims);

    /// <summary>
    /// Check if a token has the required permissions (Role.Admin)
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">If the token don't have the required permissions</exception>
    /// <param name="claims">The claims of the tokens</param>
    void CheckIfUserIsAdmin(ClaimsPrincipal claims);

    /// <summary>
    /// Check if a token has the required permissions (Role.Admin, Role.Coach)
    /// </summary>
    /// <exception cref="UnauthorizedAccessException">If the token don't have the required permissions</exception>
    /// <param name="claims">The claims of the tokens</param>
    void CheckIfUserIsAdminOrCoach(ClaimsPrincipal claims);

    /// <summary>
    /// Create a reset password token based on the given email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    string CreateResetPasswordToken(string email);
}