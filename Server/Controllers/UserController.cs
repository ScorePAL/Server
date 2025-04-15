using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.UserModel;
using ScorePALServer.Service.Interfaces;

namespace ScorePALServer.Controllers;

[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService service;

    public UserController(IUserService service)
    {
        this.service = service;
    }

    [HttpPost("register")]
    public ActionResult RegisterUser(string firstName, string lastName, string email, [FromBody] string password, long clubId)
    {
        return service.RegisterUser(firstName, lastName, email, password, clubId);
    }

    [HttpPost("login")]
    public ActionResult<User> LoginUser(string email, [FromBody] string password)
    {
        return service.LoginUser(email, password);
    }

    [Authorize]
    [HttpGet("refresh-token")]
    public ActionResult<string> GenerateNewToken()
    {
        return service.GenerateNewToken(HttpContext.User);
    }

    [HttpPost("reset-password")]
    public ActionResult ResetPassword(string email)
    {
        return service.ResetPassword(email);
    }
}