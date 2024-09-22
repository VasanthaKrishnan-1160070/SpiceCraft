using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Inventory;

namespace SpiceCraft.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController(IInventoryLogics inventoryLogics) : ControllerBase
{
    // Get all available products
    [HttpGet("products")]
    public async Task<IActionResult> GetAvailableProducts()
    {
        var result = await inventoryLogics.GetAvailableProductsAsync();
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result.Message);
    }

    // Get stock for a specific product by item ID
    [HttpGet("stock/{itemId}")]
    public async Task<IActionResult> GetProductStock(int itemId)
    {
        var result = await inventoryLogics.GetProductStockAsync(itemId);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return NotFound(result.Message);
    }

    // Update stock for a specific product by item ID
    [HttpPut("stock/update/{itemId}")]
    public async Task<IActionResult> UpdateProductStock(int itemId, [FromBody] int currentStock)
    {
        var result = await inventoryLogics.UpdateProductStockAsync(itemId, currentStock);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    // Insert a new product into the inventory
    [HttpPost("product/add")]
    public async Task<IActionResult> AddProductToInventory(int itemId, [FromBody] ProductStockUpdateDTO model)
    {
        var result = await inventoryLogics.AddProductToInventoryAsync(itemId, model.CurrentStock, model.LowStockThreshold);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }

    // Decrement stock for a specific product by a quantity
    [HttpPut("stock/decrement/{itemId}")]
    public async Task<IActionResult> DecrementProductStock(int itemId, [FromBody] int quantity)
    {
        var result = await inventoryLogics.DecrementProductStockAsync(itemId, quantity);
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        return BadRequest(result.Message);
    }
}