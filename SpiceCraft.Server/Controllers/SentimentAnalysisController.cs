using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;

namespace SpiceCraft.Server.Controllers;
[ApiController]
[Route("api/[controller]")]
public class SentimentAnalysisController : ControllerBase
{
    private readonly ISentimentAnalysisLogics _sentimentLogics;

    public SentimentAnalysisController(ISentimentAnalysisLogics sentimentLogics)
    {
        _sentimentLogics = sentimentLogics;
    }

    // POST: api/SentimentAnalysis/train
    [HttpPost("train")]
    public async Task<IActionResult> TrainModel()
    {
        try
        {
            await _sentimentLogics.TrainSentimentModelAsync();
            return Ok("Sentiment model trained successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }

    // POST: api/SentimentAnalysis/analyze
    [HttpPost("analyze")]
    public async Task<IActionResult> AnalyzeSentiment([FromBody] string ratingDescription)
    {
        if (string.IsNullOrEmpty(ratingDescription))
        {
            return BadRequest("Rating description cannot be empty");
        }

        var result = await _sentimentLogics.AnalyzeRatingDescriptionAsync(ratingDescription);
        return Ok(result);
    }
}