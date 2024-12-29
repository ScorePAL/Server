using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.DAO.Interfaces;
using VamosVamosServer.Service.Interfaces;

namespace VamosVamosServer.Service.Implementation;

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

    public ActionResult LoginUser(string email, string password)
    {
        return dao.LoginUser(email, password);
    }
}