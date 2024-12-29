using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.Model.User;

namespace VamosVamosServer.Service.Interfaces;

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
    public ActionResult LoginUser(string email, string password);
}