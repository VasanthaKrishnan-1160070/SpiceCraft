using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IUserRepository
    {
        public Task<User> GetUserByIdAsync(int userId);

        public Task<UserDTO> GetUserDetailById(int userId);
        public Task<UsersCredential> GetUserCredentialByUsernameAsync(string username,  int? userId = null);
        public Task<User> GetUserByEmailAsync(string email, int? userId = null);
        public Task<User> GetUserByUserNameAsync(string username);
        public Task<UserAddressDTO?> GetUserAddressByIdAsync(int userId);
        public Task<IEnumerable<UserDTO>> GetUserByRoleAsync(int roleId);
        public Task<bool> CreateUserAsync(CreateUserRequest userRequest);
        public Task<bool> CreateUpdateUserAsync(CreateUserRequest userRequest);
        public Task AddUserAsync(User user, UsersCredential credential, UserAddress address);
        public Task<bool> ChangePasswordAsync(ChangePasswordRequest request);
        public Task<bool> ToggleUserActiveAsync(int userId);
    }
}
