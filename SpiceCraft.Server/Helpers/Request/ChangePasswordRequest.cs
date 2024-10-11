namespace SpiceCraft.Server.Helpers.Request;

public class ChangePasswordRequest
{
    public int UserId { get; set; }
    public string NewPassword { get; set; }
    public string OldPassword { get; set; }
}