using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Logic.UserModel;

namespace ScorePALServerModel.DAO.Interfaces;

public interface IUserDAO
{
    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="userRegister">Contains all the user info when registering</param>
    /// <param name="salt">The salt to recreate the hash password</param>
    /// <returns></returns>
    public User RegisterUser(UserRegister userRegister, string salt);

    /// <summary>
    /// Make the user login
    /// </summary>
    /// <param name="userLogin"></param>
    /// <returns></returns>
    public User LoginUser(UserLogin userLogin);

    /// <summary>
    /// Get the salt from the user given the email
    /// </summary>
    /// <param name="email">The email linked to the salt</param>
    /// <returns>The salt of the user</returns>
    string GetSaltByUser(string email);
}