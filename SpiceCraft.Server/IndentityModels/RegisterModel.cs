namespace SpiceCraft.Server.IndentityModels
{
    public class RegisterModel
    {        
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public string Password { get; set; } = null;
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string ProfileImage { get; set; } = "";
        public required DateTime DateOfBirth { get; set; }
        public required string PhoneNumber { get; set; }
        public required int RoleId { get; set; }

        public required string StreetAddress1 { get; set; }
        public required string StreetAddress2 { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
    }
}
