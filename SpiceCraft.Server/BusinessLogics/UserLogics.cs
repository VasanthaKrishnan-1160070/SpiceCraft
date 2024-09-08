using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;
using System.Collections.Generic;

namespace SpiceCraft.Server.BusinessLogics
{
    public class UserLogics(IUserRepository userRepository, IConfiguration configuration) : IUserLogics
    {
        public async Task<ResultDetail<User>> GetUserByIdAsync(int userId)
        {
            var result = await userRepository.GetUserByIdAsync(userId);
            if (result == null)
            {
                return HelperFactory.Msg.Error(result, "No User Found");
            }
            return HelperFactory.Msg.Success(result);
        }

        public async Task<ResultDetail<UsersCredential>> GetUserCredentialByUsernameAsync(string username)
        {
            var result = await userRepository.GetUserCredentialByUsernameAsync(username);
            if (result == null)
            {
                return HelperFactory.Msg.Error(result, "No User Credential Found");
            }
            return HelperFactory.Msg.Success(result);
        }

        public async Task<ResultDetail<User>> GetUserByEmailAsync(string email)
        {
            var result = await userRepository.GetUserByEmailAsync(email);
            if (result == null)
            {
                return HelperFactory.Msg.Error(result, "No User Found by Email");
            }
            return HelperFactory.Msg.Success(result);
        }

        public async Task<ResultDetail<User>> GetUserByUserNameAsync(string username)
        {
            var result = await userRepository.GetUserByUserNameAsync(username);
            if (result == null)
            {
                return HelperFactory.Msg.Error(result, "No User Found by Username");
            }
            return HelperFactory.Msg.Success(result);
        }

        public async Task<ResultDetail<IEnumerable<UserDTO>>> GetUserByRole(int roleId)
        {
            var result = await userRepository.GetUserByRole(roleId);
            if (result == null || !result.Any())
            {
                return HelperFactory.Msg.Error(result, "No Users Found for the given Role");
            }
            return HelperFactory.Msg.Success< IEnumerable<UserDTO>>(result);
        }

        public async Task<ResultDetail<bool>> CreateUserAsync(CreateUserRequest userRequest)
        {
            if (userRequest != null)
            {
                string defaultPassword = configuration.GetValue<string>("Defaults:UserPassword") ?? string.Empty;
                userRequest.Password = PasswordHelper.HashPassword(userRequest?.Password ?? defaultPassword);
            }

            var result = await userRepository.CreateUserAsync(userRequest);
            if (!result)
            {
                return HelperFactory.Msg.Error(result, "User Creation Failed");
            }
            return HelperFactory.Msg.Success(result);
        }

        public async Task<ResultDetail<bool>> CreateUpdateUserAsync(CreateUserRequest userRequest)
        {
            if (userRequest != null)
            {
                string defaultPassword = configuration.GetValue<string>("Defaults:UserPassword") ?? string.Empty;
                userRequest.Password = PasswordHelper.HashPassword(userRequest?.Password ?? defaultPassword);
            }

            var result = await userRepository.CreateUpdateUserAsync(userRequest);
            if (!result)
            {
                return HelperFactory.Msg.Error(result, "User Update/Creation Failed");
            }
            return HelperFactory.Msg.Success(result);
        }

    }
}
