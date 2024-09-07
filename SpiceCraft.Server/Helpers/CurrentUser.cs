using System.Security.Claims;
using SpiceCraft.Server.Enum;

namespace SpiceCraft.Server.Helpers;

public class CurrentUser(IHttpContextAccessor _httpContextAccessor) : ICurrentUser
{
    public int UserId
    {
        get
        {
            var claim = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value == null? 0 : Convert.ToInt32(claim.Value)!;
        }
    }
    
    public UserRole UserRole
    {
        get
        {
            var claim = _httpContextAccessor?.HttpContext?.User.FindFirst(ClaimTypes.Role);
            var role = claim?.Value;
            // convert the role to the enum type
            return role switch
            {
                "Admin" => UserRole.Admin,
                "Manager" => UserRole.Manager,
                "Staff" => UserRole.Staff,
                "Customer" => UserRole.Customer,
                _ => UserRole.None
            };
        }
    }
}

public interface ICurrentUser
{
    int UserId { get; }
    UserRole UserRole { get; }
}