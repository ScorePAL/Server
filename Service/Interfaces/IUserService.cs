using Microsoft.AspNetCore.Mvc;
using Model.Logic.UserModel;

namespace ScorePALServer.Service.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Return the user with the given id
    /// </summary>
    /// <param name="token">The user's token</param>
    /// <returns></returns>
    public ActionResult GetUserByToken(string token);

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="userRegister">Contains all the user info when registering</param>
    /// <returns></returns>
    public ActionResult RegisterUser(UserRegister userRegister);

    /// <summary>
    /// Make the user login
    /// </summary>
    /// <param name="userLogin">Contains the email and the password of the client trying to connect</param>
    /// <returns></returns>
    public ActionResult<Tuple<string, string>> LoginUser(UserLogin userLogin);

    /// <summary>
    /// Generate a new token for the user
    /// </summary>
    /// <param name="refreshtoken">The refresh token of the user</param>
    /// <returns></returns>
    ActionResult<Tuple<string, string>> GenerateNewToken(string refreshtoken);
}