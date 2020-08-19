using System;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using DestinationReview.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using DestinationReview.Services;
using DestinationReview.Dtos;
using DestinationReview.Models;

namespace DestinationReview.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[Controller]")]
  public class UsersController : ControllerBase
  {
    private IUserService _userService;
    private IMapper _mapper;
    private readonly AppSettings _appSettings;
    public UsersController(IUserService userService, IMapper mapper, IOptions<AppSettings> appSettings)
    {
      _userService = userService;
      _mapper = mapper;
      _appSettings = appSettings.Value;
    }

    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authentication([FromBody]UserDto userDto)
    {
      var user = _userService.Authenticate(userDto.Username, userDto.Password);

      if (user == null)
      {
        return BadRequest(new { message = "Username or Password is Incorrecct" });
      }
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
        {
          new Claim(ClaimTypes.Name, user.Id.ToString())
        }),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenString = tokenHandler.WriteToken(token);

      return Ok(new {
        Id = user.Id,
        Username = user.Username,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Token = tokenString
      });
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult Register([FromBody]UserDto userDto)
    {
      var user = _mapper.Map<User>(userDto);
      try
      {
        _userService.Create(user, userDto.Password);
        return Ok();
      }
      catch(AppException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    [AllowAnonymous]
    [HttpGet]
    public IActionResult GetAll()
    {
      var users = _userService.GetAll();
      return Ok(users);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
      var user = _userService.GetById(id);
      var userDto = _mapper.Map<UserDto>(user);
      return Ok(userDto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody]UserDto userDto)
    {
      var user = _mapper.Map<User>(userDto);
      user.Id = id;

      try
      {
        _userService.Update(user, userDto.Password);
        return Ok();
      }
      catch(AppException ex)
      {
        return BadRequest(new { message = ex.Message });
      }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
      _userService.Delete(id);
      return Ok();
    }
  }
}