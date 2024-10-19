using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.RecentlyViewed;

namespace SpiceCraft.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecentlyViewedItemsController(IRecentlyViewedItemsLogics recentlyViewedItemsLogics) : ControllerBase
    {
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRecentlyViewedItems(int userId)
        {
            var result = await recentlyViewedItemsLogics.GetRecentlyViewedItemsAsync(userId);
            if (!result.IsSuccess) return NotFound(result.Message);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRecentlyViewedItem([FromBody] RecentlyViewedItemDTO item)
        {
            var result = await recentlyViewedItemsLogics.AddRecentlyViewedItemAsync(item);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
