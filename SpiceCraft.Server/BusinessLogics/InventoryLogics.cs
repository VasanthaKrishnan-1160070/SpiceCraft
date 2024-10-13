using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Inventory;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics;

public class InventoryLogics : IInventoryLogics
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryLogics(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    // Get all products that are not removed
    public async Task<ResultDetail<List<ProductInventoryDTO>>> GetAvailableProductsAsync()
    {
        var products = _inventoryRepository.GetProducts();
        if (products == null || !products.Any())
        {
            return HelperFactory.Msg.Error<List<ProductInventoryDTO>>("No products found.");
        }

        return HelperFactory.Msg.Success(products.ToList());
    }

    // Get the current stock for a given product ID
    public async Task<ResultDetail<int>> GetProductStockAsync(int itemId)
    {
        var stock = _inventoryRepository.GetStock(itemId);
        if (stock == 0)
        {
            return HelperFactory.Msg.Error<int>($"No stock available for Item ID: {itemId}");
        }

        return HelperFactory.Msg.Success(stock);
    }

    // Update the stock for a given product ID
    public async Task<ResultDetail<bool>> UpdateProductStockAsync(int itemId, int currentStock)
    {
        var result = _inventoryRepository.UpdateStock(itemId, currentStock);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>($"Failed to update stock for Item ID: {itemId}");
        }

        return HelperFactory.Msg.Success(true, "Stock updated successfully.");
    }

    // Insert a new product into the inventory
    public async Task<ResultDetail<bool>> AddProductToInventoryAsync(int itemId, int currentStock, int lowStockThreshold)
    {
        var result = _inventoryRepository.InsertProductToInventory(itemId, currentStock, lowStockThreshold);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to add the product to the inventory.");
        }

        return HelperFactory.Msg.Success(true, "Product added to inventory successfully.");
    }

    // Decrement the stock for a given product by a specific quantity
    public async Task<ResultDetail<int>> DecrementProductStockAsync(int itemId, int quantity)
    {
        var updatedStock = _inventoryRepository.DecrementProductStock(itemId, quantity);
        if (updatedStock < 0)
        {
            return HelperFactory.Msg.Error<int>($"Failed to decrement stock for Item ID: {itemId}");
        }

        return HelperFactory.Msg.Success(updatedStock, "Stock decremented successfully.");
    }
    
    // Get ingredients that are running low on stock
    public async Task<ResultDetail<List<IngredientDTO>>> GetLowStockIngredientsAsync()
    {
        var ingredients = await _inventoryRepository.GetLowStockIngredientsAsync();
        if (ingredients == null || !ingredients.Any())
        {
            return HelperFactory.Msg.Error<List<IngredientDTO>>("No ingredients are running low on stock.");
        }

        return HelperFactory.Msg.Success(ingredients);
    }
}