using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<UsersCredential> GetUserCredentialByUsernameAsync(string username);
        Task AddUserAsync(User user, UsersCredential credential);

        Task<User> GetUserByUserNameAsync(string username);
    }
}
