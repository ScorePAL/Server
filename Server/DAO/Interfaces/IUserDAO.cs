using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.UserModel;

namespace ScorePALServer.DAO.Interfaces;

public interface IUserDAO
{
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="firstName">The first name of the user</param>
    /// <param name="lastName">The last name of the user</param>
    /// <param name="email">The email of the user</param>
    /// <param name="password">The password hashed of the user</param>
    /// <param name="clubId">The club id of the user</param>
    /// <param name="salt">The salt to recreate the hash password</param>
    /// <returns></returns>
    public ActionResult RegisterUser(string firstName, string lastName, string email, string password, long clubId,
        string salt);

    /// <summary>
    /// Make the user login
    /// </summary>
    /// <param name="email">The user's email</param>
    /// <param name="password">The user's password</param>
    /// <returns></returns>
    public ActionResult<User> LoginUser(string email, string password);

    /// <summary>
    /// Get the salt from the user given the email
    /// </summary>
    /// <param name="email">The email linked to the salt</param>
    /// <returns>The salt of the user</returns>
    string GetSaltByUser(string email);
}