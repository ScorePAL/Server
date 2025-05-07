using Microsoft.AspNetCore.Mvc;
using Model.Logic.UserModel;

namespace Model.DAO.Interfaces;

public interface IUserDAO
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
    /// <param name="salt">The salt to recreate the hash password</param>
    /// <returns></returns>
    public ActionResult RegisterUser(UserRegister userRegister, string salt);

    /// <summary>
    /// Make the user login
    /// </summary>
    /// <param name="email">The user's email</param>
    /// <param name="password">The user's password</param>
    /// <returns></returns>
    public ActionResult<Tuple<string, string>> LoginUser(string email, string password);

    /// <summary>
    /// Generate a new token for the user
    /// </summary>
    /// <param name="refreshtoken">The refresh token of the user</param>
    /// <returns></returns>
    ActionResult<Tuple<string, string>> GenerateNewToken(string refreshtoken);

    /// <summary>
    /// Get the salt from the user given the email
    /// </summary>
    /// <param name="email">The email linked to the salt</param>
    /// <returns>The salt of the user</returns>
    string GetSaltBuYser(string email);
}