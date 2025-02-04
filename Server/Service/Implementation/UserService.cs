using Microsoft.AspNetCore.Mvc;
using ScorePALServer.DAO.Interfaces;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Service.Implementation;

public class UserService : IUserService
{
    private readonly IUserDAO dao;

    public UserService(IUserDAO dao)
    {
        this.dao = dao;
    }

    public ActionResult GetUserByToken(string token)
    {
        return dao.GetUserByToken(token);
    }

    public ActionResult RegisterUser(string firstName, string lastName, string email, string password, long clubId)
    {
        return dao.RegisterUser(firstName, lastName, email, password, clubId);
    }

    public ActionResult<Tuple<string, string>> LoginUser(string email, string password)
    {
        return dao.LoginUser(email, password);
    }

    public ActionResult<Tuple<string, string>> GenerateNewToken(string refreshtoken)
    {
        return dao.GenerateNewToken(refreshtoken);
    }
}