using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Checkout;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpiceCraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutLogics _checkoutLogics;

        public CheckoutController(ICheckoutLogics checkoutLogics)
        {
            _checkoutLogics = checkoutLogics;
        }

        // GET: api/checkout/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCheckoutInfo(int userId)
        {
            var result = await _checkoutLogics.GetCheckoutInfoAsync(userId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        // GET: api/checkout/totalamount/{userId}
        [HttpGet("totalamount/{userId}")]
        public async Task<IActionResult> GetTotalAmount(int userId, [FromQuery] int? shippingOptionId)
        {
            var result = await _checkoutLogics.GetTotalAmountToPayAsync(userId, shippingOptionId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        // POST: api/checkout/applygiftcard
        [HttpPost("applygiftcard")]
        public async Task<IActionResult> ApplyGiftCard([FromBody] ApplyGiftCardDTO request)
        {
            var result = await _checkoutLogics.ApplyGiftCardAsync(request.Code, request.UserId);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        // POST: api/checkout/startpostpayment
        [HttpPost("startpostpayment")]
        public async Task<IActionResult> StartPostPayment([FromBody] StartPostPaymentDTO request)
        {
            var result = await _checkoutLogics.StartPostPaymentProcessAsync(request.UserId, request.ShippingOptionId, request.PaymentMethod, request.GiftCardCode);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
