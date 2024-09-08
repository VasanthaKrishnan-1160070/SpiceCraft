﻿using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IUserLogics
    {
        Task<ResultDetail<User>> GetUserByIdAsync(int userId);
        Task<ResultDetail<UsersCredential>> GetUserCredentialByUsernameAsync(string username);
        Task<ResultDetail<User>> GetUserByEmailAsync(string email);
        Task<ResultDetail<User>> GetUserByUserNameAsync(string username);
        Task<ResultDetail<IEnumerable<UserDTO>>> GetUserByRole(int roleId);
        Task<ResultDetail<bool>> CreateUserAsync(CreateUserRequest userRequest);
        Task<ResultDetail<bool>> CreateUpdateUserAsync(CreateUserRequest userRequest);
    }
}
