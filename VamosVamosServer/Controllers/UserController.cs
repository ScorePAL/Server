using Microsoft.AspNetCore.Mvc;
using VamosVamosServer.Service.Interfaces;

namespace VamosVamosServer.Controllers;

[Route("api/user")]
public class UserController
{
    private readonly IUserService service;

    public UserController(IUserService service)
    {
        this.service = service;
    }

    [HttpGet("{token}")]
    public ActionResult GetUserByToken(string token)
    {
        return service.GetUserByToken(token);
    }

    [HttpPost("register")]
    public ActionResult RegisterUser(string firstName, string lastName, string email, string password, long clubId)
    {
        return service.RegisterUser(firstName, lastName, email, password, clubId);
    }

    [HttpPost("login")]
    public ActionResult<Tuple<string, string>> LoginUser(string email, string password)
    {
        return service.LoginUser(email, password);
    }

    [HttpGet("new-token")]
    public ActionResult<Tuple<string, string>> GenerateNewToken(string refreshtoken)
    {
        return service.GenerateNewToken(refreshtoken);
    }
}