using Microsoft.AspNetCore.Mvc;
using Model.Logic.UserModel;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServerController.Controllers;

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
    public ActionResult RegisterUser(UserRegister userRegister)
    {
        return service.RegisterUser(userRegister);
    }

    [HttpPost("login")]
    public ActionResult<Tuple<string, string>> LoginUser(string email, [FromBody] string password)
    {
        return service.LoginUser(email, password);
    }

    [HttpGet("new-token")]
    public ActionResult<Tuple<string, string>> GenerateNewToken(string refreshtoken)
    {
        return service.GenerateNewToken(refreshtoken);
    }
}