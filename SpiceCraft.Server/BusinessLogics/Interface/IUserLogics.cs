using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IUserLogics
    {
        Task<ResultDetail<User>> GetUserByIdAsync(int userId);
        Task<ResultDetail<UserDTO>> GetUserDetailById(int userId);
        Task<ResultDetail<UsersCredential>> GetUserCredentialByUsernameAsync(string username, int? userId);
        Task<ResultDetail<User>> GetUserByEmailAsync(string email, int? userId);
        Task<ResultDetail<User>> GetUserByUserNameAsync(string username);
        Task<ResultDetail<IEnumerable<UserDTO>>> GetUserByRoleAsync(int roleId);
        Task<ResultDetail<bool>> CreateUserAsync(CreateUserRequest userRequest);
        Task<ResultDetail<bool>> CreateUpdateUserAsync(CreateUserRequest userRequest);
    }
}
