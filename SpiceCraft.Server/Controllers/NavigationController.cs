using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using SpiceCraft.Server.BusinessLogics;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.Models;
using MLModels = SpiceCraft.Server.ML.Models.Navigation;


namespace SpiceCraft.Server.Controllers;

[ApiController]
[Route("api/navigation")]
public class NavigationController : ControllerBase
{
    private readonly SpiceCraftContext _context;
    private NavigationLogics _navigationLogics;

    public NavigationController(SpiceCraftContext context)
    {
        _context = context;
        _navigationLogics = new NavigationLogics(_context);
    }
    
    [HttpGet("predict-navigation/{userId}")]
    public async Task<IActionResult> GetNavigationPredictions(int userId)
    {
        // Use the NavigationLogics class to get the ordered navigation items
        var orderedNavigation = await _navigationLogics.GetOrderedNavigation(userId);

        if (orderedNavigation == null || !orderedNavigation.Data.Any())
        {
            return NotFound("No activity logs found for the user.");
        }

        return Ok(orderedNavigation);
    }

    [HttpPost("log-activity")]
    public async Task<IActionResult> LogUserActivity([FromBody] MLModels.UserActivityLog log)
    {
        if (log == null || log.TimeSpent < 0)
        {
            return BadRequest("Invalid activity log");
        }

        await _navigationLogics.LogUserActivity(log);

        return Ok("User activity logged successfully");
    }
}