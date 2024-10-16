using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics;
using SpiceCraft.Server.DTO.RecentlyViewed;

namespace SpiceCraft.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecentlyViewedItemsController : ControllerBase
    {
        private readonly RecentlyViewedItemsLogics _recentlyViewedItemsLogics;

        public RecentlyViewedItemsController(RecentlyViewedItemsLogics recentlyViewedItemsLogics)
        {
            _recentlyViewedItemsLogics = recentlyViewedItemsLogics;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRecentlyViewedItems(int userId)
        {
            var result = await _recentlyViewedItemsLogics.GetRecentlyViewedItemsAsync(userId);
            if (!result.IsSuccess) return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddRecentlyViewedItem([FromBody] RecentlyViewedItemDTO item)
        {
            var result = await _recentlyViewedItemsLogics.AddRecentlyViewedItemAsync(item);
            if (!result.IsSuccess) return BadRequest(result.Message);
            return Ok(result);
        }
    }
}
