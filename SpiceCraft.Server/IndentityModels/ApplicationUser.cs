using Microsoft.AspNetCore.Identity;

namespace SpiceCraft.Server.IndentityModels
{
    public class ApplicationUser : IdentityUser
    {
        // Existing Identity fields are inherited from IdentityUser

        // Add new custom fields
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}
