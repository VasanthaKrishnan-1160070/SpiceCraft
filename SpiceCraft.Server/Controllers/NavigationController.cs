using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.Models.ML.Navigation;
using System.Threading.Tasks;
using SpiceCraft.Server.Context;

namespace SpiceCraft.Server.Controllers;

[ApiController]
[Route("api/navigation")]
public class NavigationController : ControllerBase
{
    private readonly SpiceCraftContext _context;

    public NavigationController(SpiceCraftContext context)
    {
        _context = context;
    }

    [HttpPost("log")]
    public async Task<IActionResult> LogUserActivity([FromBody] UserActivityLog log)
    {
        if (log == null)
        {
            return BadRequest("Invalid activity log");
        }

        // Store user activity log in the database
        _context.UserActivityLogs.Add(log);
        await _context.SaveChangesAsync();

        return Ok("User activity logged successfully");
    }
}