using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Promotions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpiceCraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionLogics _promotionLogics;

        public PromotionController(IPromotionLogics promotionLogics)
        {
            _promotionLogics = promotionLogics;
        }

        // Get all promotions (items, categories, BOGO, bulk)
        [HttpGet("get-promotions")]
        public async Task<IActionResult> GetPromotions()
        {
            var result = await _promotionLogics.GetPromotionsAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return NotFound(result.Message);
        }
        
        // Add a new item promotion
        [HttpPost("add-item-promotion")]
        public async Task<IActionResult> AddItemPromotion([FromBody] ItemPromotionDTO promotion)
        {
            var result = await _promotionLogics.AddItemPromotionAsync(promotion);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        // Add a new category promotion
        [HttpPost("add-category-promotion")]
        public async Task<IActionResult> AddCategoryPromotion([FromBody] CategoryPromotionDTO promotion)
        {
            var result = await _promotionLogics.AddCategoryPromotionAsync(promotion);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        // Add a new BOGO promotion
        [HttpPost("add-bogo-promotion")]
        public async Task<IActionResult> AddBogoPromotion([FromBody] BogoPromotionDTO promotion)
        {
            var result = await _promotionLogics.AddBogoPromotionAsync(promotion);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        // Add a new bulk item promotion
        [HttpPost("add-bulk-item-promotion")]
        public async Task<IActionResult> AddBulkItemPromotion([FromBody] BulkPromotionDTO promotion)
        {
            var result = await _promotionLogics.AddBulkItemPromotionAsync(promotion);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        // Remove an item promotion by item ID
        [HttpDelete("remove-item-promotion/{itemId}")]
        public async Task<IActionResult> RemoveItemPromotion(int itemId)
        {
            var result = await _promotionLogics.RemoveItemPromotionAsync(itemId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        // Remove a category promotion by category ID
        [HttpDelete("remove-category-promotion/{categoryId}")]
        public async Task<IActionResult> RemoveCategoryPromotion(int categoryId)
        {
            var result = await _promotionLogics.RemoveCategoryPromotionAsync(categoryId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        // Remove a BOGO promotion by item ID
        [HttpDelete("remove-bogo-promotion/{itemId}")]
        public async Task<IActionResult> RemoveBogoPromotion(int itemId)
        {
            var result = await _promotionLogics.RemoveBogoPromotionAsync(itemId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        // Remove a bulk item promotion by item ID
        [HttpDelete("remove-bulk-item-promotion/{itemId}")]
        public async Task<IActionResult> RemoveBulkItemPromotion(int itemId)
        {
            var result = await _promotionLogics.RemoveBulkItemPromotionAsync(itemId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        // Remove all item promotions
        [HttpDelete("remove-all-item-promotions")]
        public async Task<IActionResult> RemoveAllItemPromotions()
        {
            var result = await _promotionLogics.RemoveAllItemPromotionsAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        // Remove all category promotions
        [HttpDelete("remove-all-category-promotions")]
        public async Task<IActionResult> RemoveAllCategoryPromotions()
        {
            var result = await _promotionLogics.RemoveAllCategoryPromotionsAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        // Remove all BOGO promotions
        [HttpDelete("remove-all-bogo-promotions")]
        public async Task<IActionResult> RemoveAllBogoPromotions()
        {
            var result = await _promotionLogics.RemoveAllBogoPromotionsAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }

        // Remove all bulk item promotions
        [HttpDelete("remove-all-bulk-item-promotions")]
        public async Task<IActionResult> RemoveAllBulkItemPromotions()
        {
            var result = await _promotionLogics.RemoveAllBulkItemPromotionsAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result.Message);
        }
    }
}
