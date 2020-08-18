using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DestinationReview.Services;
using DestinationReview.Models;

namespace DestinationReview.Controllers
{
  [Authorize]
  [ApiController]
  [Route("[Controller]")]
  public class UsersController : ControllerBase
  {
    private IUserService _userService;
    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authentication([FromBody]User userParam)
    {
      var user = _userService.Authenticate(userParam.Username, userParam.Password);

      if (user == null)
      {
        return BadRequest(new { message = "Username or Password is Incorrecct" });
      }

      return Ok(user);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
      var users = _userService.GetAll();
      return Ok(users);
    }
  }
}