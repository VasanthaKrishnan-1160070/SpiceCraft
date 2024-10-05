
using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SpiceCraft.Server.Controllers
{
    [ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderLogics _orderLogics;

    public OrderController(IOrderLogics orderLogics)
    {
        _orderLogics = orderLogics;
    }

    // Endpoint to place an order for a regular user
    [HttpPost("place-order/{userId}/{orderId}")]
    public async Task<IActionResult> PlaceOrder(int userId, int orderId)
    {
        var result = await _orderLogics.PlaceOrderAsync(userId, orderId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    // Endpoint to place an order for a corporate client
    [HttpPost("place-corporate-order/{userId}/{orderId}")]
    public async Task<IActionResult> PlaceCorporateOrder(int userId, int orderId)
    {
        var result = await _orderLogics.PlaceCorporateClientOrderAsync(userId, orderId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    // Endpoint to get order details
    [HttpGet("order-details/{orderId}")]
    public async Task<IActionResult> GetOrderDetails(int orderId)
    {
        var result = await _orderLogics.GetUserOrderDetailsAsync(orderId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result.Message);
    }

    // Endpoint to get all orders for a user
    [HttpGet("user-orders/{userId}")]
    public async Task<IActionResult> GetUserOrders(int userId)
    {
        var result = await _orderLogics.GetUserOrdersAsync(userId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result.Message);
    }

    // Endpoint to change order status
    [HttpPut("change-order-status/{orderId}")]
    public async Task<IActionResult> ChangeOrderStatus(int orderId, [FromBody] string newStatus)
    {
        var result = await _orderLogics.ChangeOrderStatusAsync(orderId, newStatus);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    // Endpoint to verify stock before placing an order
    [HttpGet("verify-stock/{userId}")]
    public async Task<IActionResult> VerifyStockBeforeOrder(int userId)
    {
        var result = await _orderLogics.VerifyStockBeforeOrderAsync(userId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }
}
}
