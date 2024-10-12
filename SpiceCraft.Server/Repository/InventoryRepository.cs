using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Inventory;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class InventoryRepository : IInventoryRepository
{
    private SpiceCraftContext _context;
    public InventoryRepository(SpiceCraftContext context)
    {
        _context = context;
    }

    // Get all products that are not removed
    public IEnumerable<ProductInventoryDTO> GetProducts()
    {
        var products = from p in _context.Items
                       where p.IsRemoved == false
                       select new ProductInventoryDTO
                       {
                           ItemId = p.ItemId,
                           ItemName = p.ItemName,
                           CategoryName = _context.ItemCategories
                                               .Where(pc => pc.CategoryId == p.CategoryId)
                                               .Select(pc => pc.CategoryName.ToUpper())
                                               .FirstOrDefault() ?? "",
                           ProductPrice = "$" + p.Price.ToString(),
                           AvailableStock = (from ii in _context.ItemIngredients
                                             join inv in _context.Inventories on ii.IngredientId equals inv.IngredientId
                                             where ii.ItemId == p.ItemId
                                             group new { ii, inv } by ii.ItemId into inventoryGroup
                                             select inventoryGroup.Min(g => g.inv.CurrentStock / g.ii.QuantityNeeded))
                                             .FirstOrDefault(),
                           MinimumRequiredStock = (from ii in _context.ItemIngredients
                                                   join inv in _context.Inventories on ii.IngredientId equals inv.IngredientId
                                                   where ii.ItemId == p.ItemId
                                                   group new { ii, inv } by ii.ItemId into inventoryGroup
                                                   select inventoryGroup.Min(g => g.inv.LowStockThreshold / g.ii.QuantityNeeded))
                                                   .FirstOrDefault()
                       };

        return products.ToList();
    }

    // Get the current stock for a given product ID
    public int GetStock(int itemId)
    {
        var stockInfo = (from ii in _context.ItemIngredients
                         join inv in _context.Inventories on ii.IngredientId equals inv.IngredientId
                         where ii.ItemId == itemId
                         group new { ii, inv } by ii.ItemId into inventoryGroup
                         select inventoryGroup.Min(g => g.inv.CurrentStock / g.ii.QuantityNeeded))
                         .FirstOrDefault();

        return stockInfo < 0 ? 0 : stockInfo;
    }

    // Update the stock for a given product ID
    public bool UpdateStock(int itemId, int currentStock)
    {
        var itemIngredients = _context.ItemIngredients.Where(ii => ii.ItemId == itemId).ToList();
        if (!itemIngredients.Any())
        {
            return false;
        }

        foreach (var ingredient in itemIngredients)
        {
            var inventoryItem = _context.Inventories.FirstOrDefault(i => i.IngredientId == ingredient.IngredientId);
            if (inventoryItem != null)
            {
                inventoryItem.CurrentStock = currentStock * ingredient.QuantityNeeded;
            }
        }

        _context.SaveChanges();
        return GetStock(itemId) == currentStock;
    }

    // Insert a new product into the inventory
    public bool InsertProductToInventory(int itemId, int currentStock, int lowStockThreshold)
    {
        var itemIngredients = _context.ItemIngredients.Where(ii => ii.ItemId == itemId).ToList();
        if (!itemIngredients.Any())
        {
            return false;
        }

        foreach (var ingredient in itemIngredients)
        {
            var existingItem = _context.Inventories.FirstOrDefault(i => i.IngredientId == ingredient.IngredientId);
            if (existingItem == null)
            {
                var newInventoryItem = new Inventory
                {
                    IngredientId = ingredient.IngredientId,
                    CurrentStock = currentStock * ingredient.QuantityNeeded,
                    LowStockThreshold = lowStockThreshold * ingredient.QuantityNeeded
                };

                _context.Inventories.Add(newInventoryItem);
            }
        }

        _context.SaveChanges();
        return GetStock(itemId) == currentStock;
    }

    // Decrement the stock for a given Item Id by a specified quantity
    public int DecrementProductStock(int itemId, int quantity)
    {
        var itemIngredients = _context.ItemIngredients.Where(ii => ii.ItemId == itemId).ToList();
        if (!itemIngredients.Any())
        {
            return 0;
        }

        foreach (var ingredient in itemIngredients)
        {
            var inventoryItem = _context.Inventories.FirstOrDefault(i => i.IngredientId == ingredient.IngredientId);
            if (inventoryItem != null)
            {
                inventoryItem.CurrentStock -= quantity * ingredient.QuantityNeeded;
            }
        }

        _context.SaveChanges();
        return GetStock(itemId);
    }
}
}
