using SpiceCraft.Server.DTO.User;

namespace SpiceCraft.Server.Helpers.Request
{
    public class CreateUserRequest : UserDTO
    {
        public string? Password { get; set; }
    }
}
