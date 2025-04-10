using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.UserModel;

namespace ScorePALServer.Service.Interfaces;

public interface IUserService
{
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="firstName">The first name of the user</param>
    /// <param name="lastName">The last name of the user</param>
    /// <param name="email">The email of the user</param>
    /// <param name="password">The password of the user</param>
    /// <param name="clubId">The club id of the user</param>
    /// <returns></returns>
    public ActionResult RegisterUser(string firstName, string lastName, string email, string password, long clubId);

    /// <summary>
    /// Make the user login
    /// </summary>
    /// <param name="email">The user's email</param>
    /// <param name="password">The user's password</param>
    /// <returns></returns>
    public ActionResult<User> LoginUser(string email, string password);

    /// <summary>
    /// Generate a new token for the user
    /// </summary>
    /// <returns></returns>
    ActionResult<string> GenerateNewToken(ClaimsPrincipal claims);
}