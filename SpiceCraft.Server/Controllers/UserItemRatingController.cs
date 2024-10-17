using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.UserRating;

namespace SpiceCraft.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IUserItemRatingLogics _userItemRatingLogics;

        public RatingController(IUserItemRatingLogics userItemRatingLogics)
        {
            _userItemRatingLogics = userItemRatingLogics;
        }

        [HttpPost("rate-item")]
        public async Task<IActionResult> RateItem([FromBody] UserItemRatingDTO userItemRatingDTO)
        {
            var result = await _userItemRatingLogics.AddOrUpdateRatingAsync(userItemRatingDTO);
            if (result.IsSuccess) return Ok(result);
            return BadRequest(result.Message);
        }

        [HttpGet("get-user-rating/{userId}/{itemId}")]
        public async Task<IActionResult> GetUserRating(int userId, int itemId)
        {
            var result = await _userItemRatingLogics.GetUserItemRatingAsync(userId, itemId);
            if (result.IsSuccess) return Ok(result);
            return NotFound(result.Message);
        }

        [HttpGet("get-item-ratings/{itemId}")]
        public async Task<IActionResult> GetItemRatings(int itemId)
        {
            var result = await _userItemRatingLogics.GetItemRatingsAsync(itemId);
            return Ok(result);
        }
    }
}
