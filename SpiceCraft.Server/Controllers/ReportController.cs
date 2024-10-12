using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpiceCraft.Server.Controllers;

// ReportController.cs
// ReportController.cs
[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly IReportLogics _reportLogics;

    public ReportController(IReportLogics reportLogics)
    {
        _reportLogics = reportLogics;
    }

    [HttpGet("{reportName}")]
    public async Task<IActionResult> GetReport(string reportName)
    {
        var result = await _reportLogics.GetReportByNameAsync(reportName);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result);
    }
}
