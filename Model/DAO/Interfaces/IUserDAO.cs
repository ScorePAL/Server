using Microsoft.AspNetCore.Mvc;

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