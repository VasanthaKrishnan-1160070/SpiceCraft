using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SpiceCraftContext _context;

        public UserRepository(SpiceCraftContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.Include(u => u.UsersCredential)
                                       .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UsersCredential> GetUserCredentialByUsernameAsync(string username)
        {
            return await _context.UsersCredentials.Include(uc => uc.User)
                                                 .FirstOrDefaultAsync(uc => uc.UserName == username);
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
                                 .Include(u => u.UsersCredential)
                                 .Include(r => r.Role)
                                 .Where(u => u.UsersCredential != null && u.UsersCredential.UserName == username)
                                 .FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user, UsersCredential credential)
        {
            _context.Users.Add(user);
            _context.UsersCredentials.Add(credential);
            await _context.SaveChangesAsync();
        }
    }
}
