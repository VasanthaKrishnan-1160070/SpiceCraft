using Microsoft.AspNetCore.Identity;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.IndentityModels
{
    public class ApplicationUser : IdentityUser
    {
        // Existing Identity fields are inherited from IdentityUser

        // Add new custom fields
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImg { get; set; }
        public DateTime DateOfBirth { get; set; }       
        public bool IsActive { get; set; }

        // One-to-one relationship
        //public UserAddress UserAddress { get; set; }
    }
}
