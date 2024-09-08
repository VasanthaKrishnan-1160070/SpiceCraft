using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IUserRepository
    {
        public Task<User> GetUserByIdAsync(int userId);
        public Task<UsersCredential> GetUserCredentialByUsernameAsync(string username);
        public Task<User> GetUserByEmailAsync(string email);
        public Task<User> GetUserByUserNameAsync(string username);
        public Task<IEnumerable<UserDTO>> GetUserByRole(int roleId);
        public Task<bool> CreateUserAsync(CreateUserRequest userRequest);
        public Task<bool> CreateUpdateUserAsync(CreateUserRequest userRequest);
        public Task AddUserAsync(User user, UsersCredential credential, UserAddress address);
    }
}
