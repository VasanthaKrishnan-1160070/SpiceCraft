using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Helpers.Request;
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
                .Include(u => u.UserAddresses)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<UserDTO> GetUserDetailById(int userId)
        {
            return await _context.Users.Include(u => u.UsersCredential)
                .Include(u => u.UserAddresses)
                .Where(w => w.UserId == userId)
                .Select(s => new UserDTO()
                {
                    UserId = s.UserId,
                    FirstName = s.FirstName,
                    LastName = s.LastName,
                    Email = s.Email,
                    Phone = s.Phone,
                    ProfileImg = s.ProfileImg,
                    RoleId = s.RoleId,
                    IsActive = s.IsActive,
                    UserName = s.UsersCredential.UserName,
                    DateOfBirth = s.DateofBirth != null ? s.DateofBirth.Value.ToString("dd/MM/yyyy") : string.Empty,
                    UserAddress = s.UserAddresses.Select(ua => new UserAddressDTO()
                    {
                        UserId = ua.UserId,
                        StreetAddress1 = ua.StreetAddress1,
                        StreetAddress2 = ua.StreetAddress2,
                        City = ua.City,
                        StateOrProvince = ua.StateOrProvince,
                        PostalCode = ua.PostalCode
                    }).FirstOrDefault()
                }).FirstOrDefaultAsync();
        }
        
        public async Task<UserAddressDTO?> GetUserAddressByIdAsync(int userId)
        {
            var userDetail = await GetUserByIdAsync(userId);
            var userAddress = userDetail.UserAddresses.FirstOrDefault();
            if (userAddress != null)
            {
                return new UserAddressDTO
                {
                    UserId = userDetail.UserId,
                    StreetAddress1 = userAddress.StreetAddress1,
                    StreetAddress2 = userAddress.StreetAddress2,
                    City = userAddress.City,
                    StateOrProvince = userAddress.StateOrProvince,
                    PostalCode = userAddress.PostalCode
                };
            }
            return null;
        }

        public async Task<UsersCredential> GetUserCredentialByUsernameAsync(string username, int? userId = null)
        {
            return await _context.UsersCredentials.Include(uc => uc.User)
                                                 .FirstOrDefaultAsync(uc => uc.UserName.ToLower() == username.ToLower() && (userId == null || uc.UserId!= userId));
        }

        public async Task<User> GetUserByEmailAsync(string email, int? userId=null)
        {
            return await _context.Users
                           .Include(u => u.UsersCredential)
                           .Include(u => u.UserAddresses)
                           .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && (userId == null || u.UserId!= userId));
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _context.Users
                                 .Include(u => u.UsersCredential)
                                 .Include(r => r.Role)
                                 .Where(u => u.UsersCredential != null && u.UsersCredential.UserName == username)
                                 .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetUserByRoleAsync(int roleId)
        {
            return await _context.Users
                                 .AsNoTracking()
                                 .Where(u => u.RoleId == roleId)
                                 .Select(u => new UserDTO
                                 {
                                     UserId = u.UserId,
                                     UserName = u.UsersCredential.UserName,
                                     FirstName = u.FirstName,
                                     LastName = u.LastName,
                                     Email = u.Email,
                                     RoleId = u.RoleId,
                                     IsActive = u.IsActive,
                                     Phone = u.Phone,
                                     ProfileImg = u.ProfileImg,
                                     Title = u.Title,
                                     UserAddress = u.UserAddresses
                                                .OrderBy(a => a.AddressId)
                                                .Select(a => new UserAddressDTO
                                                {
                                                    StateOrProvince = a.StateOrProvince,
                                                   City = a.City,
                                                   PostalCode = a.PostalCode,
                                                   StreetAddress1 = a.StreetAddress1,
                                                   StreetAddress2 = a.StreetAddress2,
                                                   AddressType = a.AddressType
                                                })
                                                .FirstOrDefault() ?? new UserAddressDTO()
                                 })
                                 .ToListAsync();
        }

        public async Task<bool> CreateUserAsync(CreateUserRequest userRequest)
        {
                // Create a new user
                var newUser = new User
                {
                    FirstName = userRequest.FirstName,
                    LastName = userRequest.LastName,
                    Email = userRequest.Email,
                    Phone = userRequest.Phone,
                    ProfileImg = userRequest.ProfileImg,
                    IsActive = userRequest.IsActive,
                    Title = userRequest.Title,
                    RoleId = userRequest.RoleId,
                };

                _context.Users.Add(newUser);

                // Create the user's credentials
                var newCredential = new UsersCredential
                {
                    UserName = userRequest.UserName,
                    Password = userRequest.Password, // Ensure to hash the password
                    User = newUser
                };

                _context.UsersCredentials.Add(newCredential);

                // Save changes and check if it was successful
                int rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected > 0;
        }

        public async Task<bool> CreateUpdateUserAsync(CreateUserRequest userRequest)
        {
                var existingUser = await _context.Users
                                                 .Include(u => u.UsersCredential)
                                                 .Include(u => u.UserAddresses)
                                                 .FirstOrDefaultAsync(u => u.Email == userRequest.Email);

                if (existingUser == null)
                {
                    return await CreateUserAsync(userRequest); // User not found so creating the user
                }

                // Update the user's details
                existingUser.FirstName = userRequest.FirstName;
                existingUser.LastName = userRequest.LastName;
                existingUser.Phone = userRequest.Phone;
                existingUser.ProfileImg = userRequest.ProfileImg;
                existingUser.IsActive = userRequest.IsActive;
                // existingUser.Title = userRequest.Title;
                existingUser.UpdatedAt = DateTime.UtcNow;
                

                // Update or create credentials
                if (existingUser.UsersCredential != null)
                {
                    existingUser.UsersCredential.UserName = userRequest.UserName; // Ensure to hash the password
                }
                else
                {
                    var newCredential = new UsersCredential
                    {
                        UserName = userRequest.UserName,
                        Password = userRequest.Password, // Ensure to hash the password
                        User = existingUser
                    };

                    _context.UsersCredentials.Add(newCredential);
                }
                
                // Update or create address
                if (existingUser.UserAddresses.Count == 0)
                {
                    existingUser.UserAddresses.Add(new UserAddress
                    {
                        UserId = existingUser.UserId,
                        StreetAddress1 = userRequest.UserAddress.StreetAddress1,
                        StreetAddress2 = userRequest.UserAddress.StreetAddress2,
                        City = userRequest.UserAddress.City,
                        StateOrProvince = userRequest.UserAddress.StateOrProvince,
                        PostalCode = userRequest.UserAddress.PostalCode,
                        AddressType = userRequest.UserAddress.AddressType
                    });
                }
                else
                {
                    var address = existingUser.UserAddresses.FirstOrDefault();
                    address.StreetAddress1 = userRequest.UserAddress.StreetAddress1;
                    address.StreetAddress2 = userRequest.UserAddress.StreetAddress2;
                    address.City = userRequest.UserAddress.City;
                    address.StateOrProvince = userRequest.UserAddress.StateOrProvince;
                    address.PostalCode = userRequest.UserAddress.PostalCode;
                }

                // Save changes and check if it was successful
                int rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected > 0;            
        }

        public async Task AddUserAsync(User user, UsersCredential credential, UserAddress address)
        {
            _context.Users.Add(user);
            _context.UsersCredentials.Add(credential);
            _context.UserAddresses.Add(address);
            await _context.SaveChangesAsync();
        }
    }
}
