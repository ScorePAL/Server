using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Logic.UserModel;

namespace ScorePALServerService.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="userRegister">Contains all the user info when registering</param>
    /// <returns></returns>
    public User RegisterUser(UserRegister userRegister);

    /// <summary>
    /// Make the user login
    /// </summary>
    /// <param name="userLogin">Contains the email and the password of the client trying to connect</param>
    /// <returns></returns>
    public ActionResult<User> LoginUser(UserLogin userLogin);

    /// <summary>
    /// Generate a new token for the user
    /// </summary>
    /// <param name="claims"></param>
    /// <returns></returns>
    ActionResult<string> GenerateNewToken(ClaimsPrincipal claims);

    /// <summary>
    /// Send a mail to the given email with a link to reset his password
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    ActionResult ResetPassword(string email);
}