using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScorePALServer.Model.UserModel;
using ScorePALServerModel.Logic.UserModel;
using ScorePALServerService.Interfaces;

namespace ScorePALServerController.Controllers;

[Route("api/user")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpPost("register")]
    public User RegisterUser(UserRegister userRegister)
    {
        return service.RegisterUser(userRegister);
    }

    [HttpPost("login")]
    public User LoginUser(UserLogin userLogin)
    {
        return service.LoginUser(userLogin);
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