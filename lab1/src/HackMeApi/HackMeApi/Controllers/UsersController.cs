using HackMeApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using HackMeApi.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace HackMeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserService _userService;

        public UsersController(ILogger<UsersController> logger, UserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login([FromBody, Required] LoginRequestDto loginRequest)
        {
            var user = _userService.GetUserFromLogin(loginRequest.Username, loginRequest.Password);
            if (user is null)
                return Unauthorized("Invalid Credentials");

            // get user from db
            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.UserName
            };

            var claims = new List<Claim>
            {
                new(ClaimTypes.PrimarySid, userDto.Id.ToString()),
                new(ClaimTypes.Name, userDto.Username),
                //new(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            _logger.LogInformation(
                "User {Email} logged in at {Time}.",
                userDto.Username, DateTime.UtcNow
            );
            return Ok(userDto);
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register([FromBody, Required] LoginRequestDto loginRequest)
        {
            // get user from db
            var user = await _userService.RegisterUser(loginRequest.Username, loginRequest.Password);
            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Username = user.UserName,
            };

            var claims = new List<Claim>
            {
                new(ClaimTypes.PrimarySid, userDto.Id.ToString()),
                new(ClaimTypes.Name, userDto.Username),
                //new(ClaimTypes.Role, "Administrator"),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity)
            );

            _logger.LogInformation(
                "User {Email} logged in at {Time}.",
                userDto.Username, DateTime.UtcNow
            );
            return Ok(userDto);
        }

        // A5:2017-Broken Access Control
        // should use RBAC to prevent that all users can delete other users.
        // fix: [Authorize(Roles = "Administrator")] by Robert & Mathis
        [Authorize(Roles = "Administrator")]
        [Authorize]
        [HttpDelete("/{userName}")]
        public async Task DeleteUser(string userName)
        {
            await _userService.DeleteUser(userName);
        }
    }
}