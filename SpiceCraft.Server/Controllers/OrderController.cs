
using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Order;

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
    
    [HttpPut("update-order/{orderId}")]
    public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] UserOrderDetailDTO orderDetail)
    {
        var result = await _orderLogics.UpdateOrderAsync(orderId, orderDetail);
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

    [HttpGet("all-orders")]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _orderLogics.GetOrdersAsync();
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
    
    // 1. Get Today's Orders Count
    [HttpGet("count/today")]
    public async Task<IActionResult> GetTodaysOrdersCount()
    {
        var result = await _orderLogics.GetTodaysOrdersCountAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    // 2. Get Orders That Need to Be Shipped
    [HttpGet("count/to-ship")]
    public async Task<IActionResult> GetOrdersToShipCount()
    {
        var result = await _orderLogics.GetOrdersToShipCountAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    // 3. Get Total Sales Today
    [HttpGet("sales/today")]
    public async Task<IActionResult> GetTotalSalesToday()
    {
        var result = await _orderLogics.GetTotalSalesTodayAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    // 4. Get Total Sales This Month
    [HttpGet("sales/month")]
    public async Task<IActionResult> GetTotalSalesThisMonth()
    {
        var result = await _orderLogics.GetTotalSalesThisMonthAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }
}
}
