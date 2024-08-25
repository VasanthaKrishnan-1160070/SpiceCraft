using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<UsersCredential> GetUserCredentialByUsernameAsync(string username);

        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user, UsersCredential credential);
        Task AddUserAsync(User user, UsersCredential credential, UserAddress userAddress);

        Task<User> GetUserByUserNameAsync(string username);
    }
}
