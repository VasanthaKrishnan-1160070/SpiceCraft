using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;
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
        private readonly IUserLogics _userLogics;

        public UserController(IUserRepository userRepository, IUserLogics userLogics)
        {
            _userRepository = userRepository;
            _userLogics = userLogics;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return Ok();
        }

        [HttpGet("check-username/{userName}")]
        public async Task<IActionResult> CheckUserName(string userName, int? userId = null)
        {
            var existingUser = await _userRepository.GetUserCredentialByUsernameAsync(userName, userId);
            if (existingUser != null)
            {
                return Ok(new { valid = false });
            }

            return Ok(new { valid = true });
        }
        
        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetUserDetailById(int userId)
        {
            var result = await _userLogics.GetUserDetailById(userId);
            return Ok(result);
        }

        [HttpGet("check-email/{email}")]
        public async Task<IActionResult> CheckEmail(string email, int? userId = null)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(email, userId);
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

        [HttpPost("create-or-update")]
        public async Task<IActionResult> CreateOrUpdateUser([FromBody] CreateUserRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var result = await _userLogics.CreateUpdateUserAsync(userRequest);
            return Ok(result);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            return Ok(); // TODO: Implement password change logic
        }
        
        [HttpPost("toggle-user-active/{userId:int}")]
        public async Task<IActionResult> ToggleUserActive(int userId)
        {
            await _userLogics.ToggleUserActiveAsync(userId);
            return Ok(new { success = true, message = "User deleted successfully" });
        }
    }
}
