using SpiceCraft.Server.DTO.Inventory;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IInventoryLogics
{
    // Get all available products
    Task<ResultDetail<List<ProductInventoryDTO>>> GetAvailableProductsAsync();

    // Get the current stock for a given product by item ID
    Task<ResultDetail<int>> GetProductStockAsync(int itemId);

    // Update the stock for a given product by item ID
    Task<ResultDetail<bool>> UpdateProductStockAsync(int itemId, int currentStock);

    // Insert a new product into the inventory
    Task<ResultDetail<bool>> AddProductToInventoryAsync(int itemId, int currentStock, int lowStockThreshold);

    // Decrement the stock for a given product by a specific quantity
    Task<ResultDetail<int>> DecrementProductStockAsync(int itemId, int quantity);
    
    Task<ResultDetail<List<IngredientDTO>>> GetLowStockIngredientsAsync();

    Task<ResultDetail<IEnumerable<IngredientInventoryDTO>>> GetInventory();

    void UpdateInventoryStock(int ingredientId, int newStock);
}