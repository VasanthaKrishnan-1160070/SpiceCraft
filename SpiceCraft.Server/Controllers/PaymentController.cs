using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpiceCraft.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentLogics _paymentLogic;

        public PaymentController(IPaymentLogics paymentBusinessLogic)
        {
            _paymentLogic = paymentBusinessLogic;
        }

        // Get payments for a specific user
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPaymentsForUser(int userId)
        {
            var result = await _paymentLogic.GetPaymentsForUserAsync(userId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }

        [HttpGet("all-payments")]
        public async Task<IActionResult> GetAllPayments()
        {
            var result = await _paymentLogic.GetPaymentsForInternalUsersAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }


        // Get payments for internal users
        [HttpGet("internal")]
        public async Task<IActionResult> GetPaymentsForInternalUsers()
        {
            var result = await _paymentLogic.GetPaymentsForInternalUsersAsync();
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }

        // Get payment invoice details by transaction ID
        [HttpGet("invoice/{transactionId}")]
        public async Task<IActionResult> GetPaymentInvoiceDetails(int transactionId)
        {
            var result = await _paymentLogic.GetPaymentInvoiceDetailsAsync(transactionId);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result.Message);
        }
    }
}
