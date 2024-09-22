using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.DTO.User
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty; // Default to empty string
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }

        public bool IsActive { get; set; } = false;
        public string Phone { get; set; } = string.Empty;
        public string ProfileImg { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public UserAddressDTO UserAddress { get; set; } = new UserAddressDTO(); // Default to new UserAddressDTO

    }
}
