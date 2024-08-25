using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Microsoft.IdentityModel.Tokens;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.IndentityModels;
using SpiceCraft.Server.Middleware;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SpiceCraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public AuthenticationController(IUserRepository userRepository, JwtService jwtService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var existingUser = await _userRepository.GetUserCredentialByUsernameAsync(model.UserName);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username is already taken" });
            }

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };

            var credential = new UsersCredential
            {
                UserName = model.UserName,
                Password = PasswordHelper.HashPassword(model.Password),
                User = user
            };

            await _userRepository.AddUserAsync(user, credential);

            return Ok(new { message = "User registered successfully" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userRepository.GetUserByUserNameAsync(model.Username);

            if (user != null && PasswordHelper.VerifyPassword(model.Password, user?.UsersCredential?.Password))
            {
                var token = _jwtService.GenerateToken(user, user.UsersCredential, user.Role.RoleName);
                var resp = new {token, user.Role.RoleName, user.FirstName};
                return Ok(resp);
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
