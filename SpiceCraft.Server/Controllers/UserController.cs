using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.IndentityModels;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpiceCraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok();
        }

        [HttpGet("check-username/{userName}")]
        public async Task<IActionResult> CheckUserName(string userName)
        {
            var existingUser = await _userRepository.GetUserCredentialByUsernameAsync(userName);
            if (existingUser != null)
            {
                return Ok(new { valid = false });
            }

            return Ok(new { valid = true });
        }

        [HttpGet("check-email/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email);
            if (existingUser != null)
            {
                return Ok(new { valid = false });
            }

            return Ok(new { valid = true });
        }

        [HttpGet("user-role/{roleId:int}")]
        public async Task<IEnumerable<UserDTO>> GetUserByRole(int roleId)
        {
            return await _userRepository.GetUserByRoleAsync(roleId);
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
                IsActive = true,
                Phone = model.PhoneNumber,
                ProfileImg = model.ProfileImage,
                RoleId = model.RoleId,
            };

            var credential = new UsersCredential
            {
                UserName = model.UserName,
                Password = PasswordHelper.HashPassword(model.Password),
                User = user
            };

            var address = new UserAddress
            {
                StreetAddress1 = model.StreetAddress1,
                StreetAddress2 = model.StreetAddress2,
                City = model.City,
                StateOrProvince = model.State,
                PostalCode = model.PostalCode               
            };

            await _userRepository.AddUserAsync(user, credential, address);

            return Ok(new { success = true, message = "User registered successfully" });
        }
    }
}
