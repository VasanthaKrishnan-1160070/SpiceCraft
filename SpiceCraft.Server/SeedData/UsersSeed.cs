using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SpiceCraft.Server.IndentityModels;
using SpiceCraft.Server.Context;

namespace SpiceCraft.Server.SeedData
{
    public class UsersSeed
    {
        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<SpiceCraftContext>();
            var password = "SpiceCraft@123";

            // Seed roles
            var roles = new[] { "Admin", "Manager", "Staff", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed admin user
            var adminUser = await userManager.FindByEmailAsync("admin@spicecrafttest.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@spicecrafttest.com",
                    FirstName = "Admin",
                    LastName = "User",
                    DateOfBirth = new DateTime(1980, 1, 1)                   
                };
                var result = await userManager.CreateAsync(adminUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var staffUser = await userManager.FindByEmailAsync("staff@spicecrafttest.com");
            if (staffUser == null)
            {
                staffUser = new ApplicationUser
                {
                    UserName = "staff",
                    Email = "staff@spicecrafttest.com",
                    FirstName = "Staff",
                    LastName = "User",
                    DateOfBirth = new DateTime(1980, 1, 1)
                    
                };
                var result = await userManager.CreateAsync(staffUser, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(staffUser, "Staff");
                }
            }

            var customer = await userManager.FindByEmailAsync("customer@spicecrafttest.com");
            if (customer == null)
            {
                customer = new ApplicationUser
                {
                    UserName = "customer",
                    Email = "customer@spicecrafttest.com",
                    FirstName = "Customer",
                    LastName = "User",
                    DateOfBirth = new DateTime(1980, 1, 1)
                   
                };
                var result = await userManager.CreateAsync(customer, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, "Customer");
                }
            }

            var manager = await userManager.FindByEmailAsync("manager@spicecrafttest.com");
            if (manager == null)
            {
                customer = new ApplicationUser
                {
                    UserName = "manager",
                    Email = "manager@spicecrafttest.com",
                    FirstName = "Manager",
                    LastName = "User",
                    DateOfBirth = new DateTime(1980, 1, 1)                   
                };
                var result = await userManager.CreateAsync(customer, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, "Manager");
                }
            }
        }
    }
}
