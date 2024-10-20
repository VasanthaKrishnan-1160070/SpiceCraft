using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;

namespace SpiceCraft.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ICustomerDashboardLogics _dashboardLogics;

    public DashboardController(ICustomerDashboardLogics dashboardLogics)
    {
        _dashboardLogics = dashboardLogics;
    }

    // GET: api/CustomerDashboard/{userId}
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCustomerDashboard(int userId)
    {
        try
        {
            // Call the business logic to get the dashboard data
            var dashboardData = await _dashboardLogics.GetCustomerDashboardDataAsync(userId);
            
            // Return the data in a successful response
            return Ok(dashboardData);
        }
        catch (Exception ex)
        {
            // Log the error (you can implement logging here) and return a generic error message
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}
