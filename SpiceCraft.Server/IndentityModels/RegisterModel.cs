namespace SpiceCraft.Server.IndentityModels
{
    public class RegisterModel
    {
        public string Title { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateOfBirth { get; set; }        
    }
}
