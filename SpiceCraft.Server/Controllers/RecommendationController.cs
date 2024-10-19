using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;

namespace SpiceCraft.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecommendationController : ControllerBase
{
    private readonly IRecommendationLogics _recommendationLogics;

    public RecommendationController(IRecommendationLogics recommendationLogics)
    {
        _recommendationLogics = recommendationLogics;
    }

    // POST: api/Recommendation/train
    [HttpPost("train")]
    public async Task<IActionResult> TrainModel()
    {
        try
        {
            await _recommendationLogics.TrainRecommendationModelAsync();
            return Ok("Model trained successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    // GET: api/Recommendation/user/{userId}/top/{topN}
    [HttpGet("user/{userId}/top/{topN}")]
    public async Task<IActionResult> GetRecommendations(int userId, int topN)
    {
        try
        {
            var recommendations = await _recommendationLogics.GetRecommendedItemsAsync(userId, topN);
            return Ok(recommendations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}
