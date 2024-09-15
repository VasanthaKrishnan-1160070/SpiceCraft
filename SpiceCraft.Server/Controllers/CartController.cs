using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpiceCraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartLogics _shoppingCartBusinessLogic;

        public CartController(ICartLogics shoppingCartBusinessLogic)
        {
            _shoppingCartBusinessLogic = shoppingCartBusinessLogic;
        }

        // Retrieves shopping cart for a specific user
        [HttpGet("user/{userId}/cart")]
        public async Task<IActionResult> GetShoppingCartByUserId(int userId)
        {
            var result = await _shoppingCartBusinessLogic.GetShoppingCartByUserIdAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(new { data = result.Data, message = result.Message });
            }

            return NotFound(new { message = result.Message });
        }

        // Retrieves shopping cart for a corporate client
        [HttpGet("user/{userId}/corporate-cart")]
        public async Task<IActionResult> GetShoppingCartForCorporateClients(int userId)
        {
            var result = await _shoppingCartBusinessLogic.GetShoppingCartForCorporateClientsAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(new { data = result.Data, message = result.Message });
            }

            return NotFound(new { message = result.Message });
        }

        // Retrieves total cart price for a specific user
        [HttpGet("user/{userId}/total-price")]
        public async Task<IActionResult> GetTotalCartPrice(int userId)
        {
            var result = await _shoppingCartBusinessLogic.GetTotalCartPriceAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(new { totalPrice = result.Data, message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }

        // Increments the quantity of a specific cart item
        [HttpPost("cart-item/{cartItemId}/increment")]
        public async Task<IActionResult> IncrementCartItemQuantity(int cartItemId, [FromQuery] int quantity)
        {
            var result = await _shoppingCartBusinessLogic.IncrementCartItemQuantityAsync(cartItemId, quantity);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }

        // Decrements the quantity of a specific cart item
        [HttpPost("cart-item/{cartItemId}/decrement")]
        public async Task<IActionResult> DecrementCartItemQuantity(int cartItemId, [FromQuery] int quantity)
        {
            var result = await _shoppingCartBusinessLogic.DecrementCartItemQuantityAsync(cartItemId, quantity);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }

        // Adds or updates a cart item
        [HttpPost("cart-item")]
        public async Task<IActionResult> AddOrUpdateCartItem([FromBody] CartItemDTO cartItemDTO)
        {
            var result = await _shoppingCartBusinessLogic.AddOrUpdateCartItemAsync(cartItemDTO);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }

        // Removes a cart item
        [HttpDelete("cart-item/{cartItemId}")]
        public async Task<IActionResult> RemoveCartItem(int cartItemId)
        {
            var result = await _shoppingCartBusinessLogic.RemoveCartItemAsync(cartItemId);

            if (result.IsSuccess)
            {
                return Ok(new { message = result.Message });
            }

            return BadRequest(new { message = result.Message });
        }
    }
}
