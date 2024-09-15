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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userRepository.GetUserByUserNameAsync(model.Username);

            if (user != null && PasswordHelper.VerifyPassword(model.Password, user?.UsersCredential?.Password))
            {
                var token = _jwtService.GenerateToken(user, user.UsersCredential, user.Role.RoleName);
                var resp = new {token, user.Role.RoleName, user.FirstName, user.UserId, user.LastName, user.Email};
                return Ok(resp);
            }

            return Unauthorized(new { message = "Invalid credentials" });
        }
    }
}
