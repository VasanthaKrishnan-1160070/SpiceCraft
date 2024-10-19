using Microsoft.EntityFrameworkCore;
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
                            group new { ii, inv } by ii.ItemId
                            into inventoryGroup
                            select inventoryGroup.Min(g => g.inv.CurrentStock / g.ii.QuantityNeeded))
                        .FirstOrDefault(),
                    MinimumRequiredStock = (from ii in _context.ItemIngredients
                            join inv in _context.Inventories on ii.IngredientId equals inv.IngredientId
                            where ii.ItemId == p.ItemId
                            group new { ii, inv } by ii.ItemId
                            into inventoryGroup
                            select inventoryGroup.Min(g => g.inv.LowStockThreshold / g.ii.QuantityNeeded))
                        .FirstOrDefault(),

                    // Updated ingredients information here
                    Ingredients = (from ii in _context.ItemIngredients
                        join ing in _context.Ingredients on ii.IngredientId equals ing.IngredientId
                        join inv in _context.Inventories on ii.IngredientId equals inv.IngredientId
                        where ii.ItemId == p.ItemId
                        select new IngredientDTO
                        {
                            IngredientId = ing.IngredientId,
                            IngredientName = ing.IngredientName,
                            CurrentStock = inv.CurrentStock,
                            ReorderLevel = inv.LowStockThreshold,
                            Unit = ing.Unit,
                            ItemsPerUnit = ing.ItemsPerUnit,
                            QuantityNeeded = ii.QuantityNeeded
                        }).ToList()
                };

            return products.ToList();
        }


        // Method to fetch the inventory details
        public async Task<IEnumerable<IngredientInventoryDTO>> GetInventory()
        {
            var inventory = from inv in _context.Inventories
                join ing in _context.Ingredients on inv.IngredientId equals ing.IngredientId
                select new IngredientInventoryDTO
                {
                    IngredientId = ing.IngredientId,
                    IngredientName = ing.IngredientName,
                    Unit = ing.Unit,
                    ItemsPerUnit = ing.ItemsPerUnit,
                    NumberOfUnits = Math.Round((double)inv.CurrentStock / ing.ItemsPerUnit, 1),
                    CurrentStock = inv.CurrentStock,
                    ReorderLevel = inv.LowStockThreshold
                };

            var result = await inventory.ToListAsync();
            return result;
        }
        
        // Method to update the CurrentStock of an ingredient
        public void UpdateInventoryStock(int ingredientId, int newStock)
        {
            var inventory = _context.Inventories.FirstOrDefault(i => i.IngredientId == ingredientId);
            if (inventory != null)
            {
                inventory.CurrentStock = newStock;
                inventory.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }
        }

        // Get the current stock for a given product ID
        public int GetStock(int itemId)
        {
            var stockInfo = (from ii in _context.ItemIngredients
                    join inv in _context.Inventories on ii.IngredientId equals inv.IngredientId
                    where ii.ItemId == itemId
                    group new { ii, inv } by ii.ItemId
                    into inventoryGroup
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
        
        public async Task<List<IngredientDTO>> GetLowStockIngredientsAsync()
        {
            var lowStockIngredients = await _context.Inventories
                .Include(i => i.Ingredient)
                .Where(i => i.CurrentStock <= i.LowStockThreshold)
                .Select(i => new IngredientDTO
                {
                    IngredientId = i.IngredientId,
                    IngredientName = i.Ingredient.IngredientName,
                    CurrentStock = i.CurrentStock,
                    ReorderLevel = i.LowStockThreshold
                })
                .ToListAsync();

            return lowStockIngredients;
        }
    }
}
