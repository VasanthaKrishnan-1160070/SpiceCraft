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
            var password = "Apple123!";

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
            if (!context.Users.Any())
            {
               var users = new[]
                {
                    new User { Title = "Mr.", FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Phone = "1234567890", ProfileImg = "img1.jpg", RoleId = 1, IsActive = true, DateofBirth = new DateOnly(1980, 5, 15) },
                    new User { Title = "Ms.", FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Phone = "0987654321", ProfileImg = "img2.jpg", RoleId = 2, IsActive = true, DateofBirth = new DateOnly(1985, 9, 23) },
                    new User { Title = "Dr.", FirstName = "Alan", LastName = "Walker", Email = "alan.walker@example.com", Phone = "1112223333", ProfileImg = "img3.jpg", RoleId = 3, IsActive = true, DateofBirth = new DateOnly(1978, 7, 30) },
                    new User { Title = "Mr.", FirstName = "Mark", LastName = "Johnson", Email = "mark.johnson@example.com", Phone = "4445556666", ProfileImg = "img4.jpg", RoleId = 4, IsActive = true, DateofBirth = new DateOnly(1982, 11, 11) },
                    new User { Title = "Ms.", FirstName = "Sarah", LastName = "Connor", Email = "sarah.connor@example.com", Phone = "7778889999", ProfileImg = "img5.jpg", RoleId = 4, IsActive = true, DateofBirth = new DateOnly(1987, 2, 20) },
                    new User { Title = "Dr.", FirstName = "Emma", LastName = "Davis", Email = "emma.davis@example.com", Phone = "2223334444", ProfileImg = "img6.jpg", RoleId = 2, IsActive = true, DateofBirth = new DateOnly(1975, 4, 18) },
                    new User { Title = "Mr.", FirstName = "Luke", LastName = "Skywalker", Email = "luke.skywalker@example.com", Phone = "5556667777", ProfileImg = "img7.jpg", RoleId = 3, IsActive = true, DateofBirth = new DateOnly(1983, 6, 10) },
                    new User { Title = "Ms.", FirstName = "Leia", LastName = "Organa", Email = "leia.organa@example.com", Phone = "8889990000", ProfileImg = "img8.jpg", RoleId = 4, IsActive = true, DateofBirth = new DateOnly(1983, 6, 10) },
                    new User { Title = "Mr.", FirstName = "Han", LastName = "Solo", Email = "han.solo@example.com", Phone = "1110002222", ProfileImg = "img9.jpg", RoleId = 4, IsActive = true, DateofBirth = new DateOnly(1975, 7, 13) },
                    new User { Title = "Ms.", FirstName = "Rey", LastName = "Palpatine", Email = "rey.palpatine@example.com", Phone = "3334445555", ProfileImg = "img10.jpg", RoleId = 1, IsActive = true, DateofBirth = new DateOnly(1993, 12, 12) },
                    new User { Title = "Mr.", FirstName = "Kylo", LastName = "Ren", Email = "kylo.ren@example.com", Phone = "6667778888", ProfileImg = "img11.jpg", RoleId = 3, IsActive = true, DateofBirth = new DateOnly(1988, 1, 1) },
                    new User { Title = "Ms.", FirstName = "Padme", LastName = "Amidala", Email = "padme.amidala@example.com", Phone = "9990001111", ProfileImg = "img12.jpg", RoleId = 2, IsActive = true, DateofBirth = new DateOnly(1981, 3, 17) },
                    new User { Title = "Dr.", FirstName = "Yoda", LastName = "Unknown", Email = "yoda.unknown@example.com", Phone = "2221113333", ProfileImg = "img13.jpg", RoleId = 2, IsActive = true, DateofBirth = new DateOnly(1900, 1, 1) },
                    new User { Title = "Mr.", FirstName = "Obi-Wan", LastName = "Kenobi", Email = "obiwan.kenobi@example.com", Phone = "4445556666", ProfileImg = "img14.jpg", RoleId = 4, IsActive = true, DateofBirth = new DateOnly(1964, 3, 25) },
                    new User { Title = "Ms.", FirstName = "Ahsoka", LastName = "Tano", Email = "ahsoka.tano@example.com", Phone = "7778889999", ProfileImg = "img15.jpg", RoleId = 4, IsActive = true, DateofBirth = new DateOnly(1984, 10, 14) },
                    new User { Title = "Mr.", FirstName = "Anakin", LastName = "Skywalker", Email = "anakin.skywalker@example.com", Phone = "1234567890", ProfileImg = "img16.jpg", RoleId = 3, IsActive = true, DateofBirth = new DateOnly(1981, 5, 22) },
                    new User { Title = "Ms.", FirstName = "Mace", LastName = "Windu", Email = "mace.windu@example.com", Phone = "0987654321", ProfileImg = "img17.jpg", RoleId = 1, IsActive = true, DateofBirth = new DateOnly(1972, 11, 6) },
                    new User { Title = "Dr.", FirstName = "Qui-Gon", LastName = "Jinn", Email = "quigon.jinn@example.com", Phone = "1112223333", ProfileImg = "img18.jpg", RoleId = 4, IsActive = true, DateofBirth = new DateOnly(1960, 8, 19) },
                    new User { Title = "Mr.", FirstName = "Jar Jar", LastName = "Binks", Email = "jarjar.binks@example.com", Phone = "4445556666", ProfileImg = "img19.jpg", RoleId = 4, IsActive = true, DateofBirth = new DateOnly(1973, 5, 4) },
                    new User { Title = "Ms.", FirstName = "Darth", LastName = "Maul", Email = "darth.maul@example.com", Phone = "7778889999", ProfileImg = "img20.jpg", RoleId = 3, IsActive = true, DateofBirth = new DateOnly(1977, 9, 16) }
                };


                context.Users.AddRange(users);
                await context.SaveChangesAsync();
            }

            // Seed user credentials
            if (!context.UsersCredentials.Any())
            {
                var credentials = new[]
                {
                new UsersCredential { UserName = "john_doe", Password = PasswordHelper.HashPassword(password), UserId = 1 },
                new UsersCredential { UserName = "jane_smith", Password = PasswordHelper.HashPassword(password), UserId = 2 },
                new UsersCredential { UserName = "alan_walker", Password = PasswordHelper.HashPassword(password), UserId = 3 },
                new UsersCredential { UserName = "mark_johnson", Password = PasswordHelper.HashPassword(password), UserId = 4 },
                new UsersCredential { UserName = "sarah_connor", Password = PasswordHelper.HashPassword(password), UserId = 5 },
                new UsersCredential { UserName = "emma_davis", Password = PasswordHelper.HashPassword(password), UserId = 6 },
                new UsersCredential { UserName = "luke_skywalker", Password = PasswordHelper.HashPassword(password), UserId = 7 },
                new UsersCredential { UserName = "leia_organa", Password = PasswordHelper.HashPassword(password), UserId = 8 },
                new UsersCredential { UserName = "han_solo", Password = PasswordHelper.HashPassword(password), UserId = 9 },
                new UsersCredential { UserName = "rey_palpatine", Password = PasswordHelper.HashPassword(password), UserId = 10 },
                new UsersCredential { UserName = "kylo_ren", Password = PasswordHelper.HashPassword(password), UserId = 11 },
                new UsersCredential { UserName = "padme_amidala", Password = PasswordHelper.HashPassword(password), UserId = 12 },
                new UsersCredential { UserName = "yoda_unknown", Password = PasswordHelper.HashPassword(password), UserId = 13 },
                new UsersCredential { UserName = "obiwan_kenobi", Password = PasswordHelper.HashPassword(password), UserId = 14 },
                new UsersCredential { UserName = "ahsoka_tano", Password = PasswordHelper.HashPassword(password), UserId = 15 },
                new UsersCredential { UserName = "anakin_skywalker", Password = PasswordHelper.HashPassword(password), UserId = 16 },
                new UsersCredential { UserName = "mace_windu", Password = PasswordHelper.HashPassword(password), UserId = 17 },
                new UsersCredential { UserName = "quigon_jinn", Password = PasswordHelper.HashPassword(password), UserId = 18 },
                new UsersCredential { UserName = "jarjar_binks", Password = PasswordHelper.HashPassword(password), UserId = 19 },
                new UsersCredential { UserName = "darth_maul", Password = PasswordHelper.HashPassword(password), UserId = 20 }
            };

                context.UsersCredentials.AddRange(credentials);
                await context.SaveChangesAsync();
            }

            // Seed main users
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
                    RoleId = role.RoleId,
                    DateofBirth = new DateOnly(1990, 1, 1),
                    IsActive = true,
                    ProfileImg = "default_profile.jpg",
                    Phone = "1234567890"
                };

                var credential = new UsersCredential
                {
                    UserName = username,
                    Password = PasswordHelper.HashPassword(password),
                    User = user
                };

                var address = new UserAddress
                {
                    UserId = user.UserId,
                    StreetAddress1 = "123 Main St",
                    StreetAddress2 = "Apt 4B",
                    City = "New York",
                    StateOrProvince = "NY",
                    PostalCode = "1234",
                    AddressType = "shipping"
                };
                
                user.UsersCredential = credential;
                user.UserAddresses.Add(address);

                context.Users.Add(user);
                // context.UsersCredentials.Add(credential); 
                // context.UserAddresses.Add(address);

                await context.SaveChangesAsync();
            }
        }

    }
}
