using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SpiceCraft.Server.IndentityModels;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace SpiceCraft.Server.SeedData
{
    public class UsersSeed
    {
        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<SpiceCraftContext>();
            var password = "SpiceCraft@123";

            // Seed roles
            var roles = new[] { "Admin", "Manager", "Staff", "Customer" };
            foreach (var roleName in roles)
            {
                if (!context.Roles.Any(r => r.RoleName == roleName))
                {
                    var role = new Role { RoleName = roleName };
                    context.Roles.Add(role);
                    await context.SaveChangesAsync();
                }
            }

            // Seed users
            await SeedUserAsync(context, "admin@spicecrafttest.com", "admin", "Admin", "User", "Admin", password);
            await SeedUserAsync(context, "staff@spicecrafttest.com", "staff", "Staff", "User", "Staff", password);
            await SeedUserAsync(context, "customer@spicecrafttest.com", "customer", "Customer", "User", "Customer", password);
            await SeedUserAsync(context, "manager@spicecrafttest.com", "manager", "Manager", "User", "Manager", password);
        }

        private static async Task SeedUserAsync(SpiceCraftContext context, string email, string username, string firstName, string lastName, string roleName, string password)
        {
            if (!context.UsersCredentials.Any(uc => uc.UserName == username))
            {

                // Find the RoleId based on the role name
                var role = await context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
                if (role == null)
                {
                    throw new Exception($"Role '{roleName}' not found in the database.");
                }

                var user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    RoleId = role.RoleId
                };

                var credential = new UsersCredential
                {
                    UserName = username,
                    Password = PasswordHelper.HashPassword(password),
                    User = user
                };

                context.Users.Add(user);
                context.UsersCredentials.Add(credential);               

                await context.SaveChangesAsync();
            }
        }

    }
}
