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
                           join i in _context.Inventories on p.ItemId equals i.ItemId
                           where p.IsRemoved == false
                           select new ProductInventoryDTO
                           {
                               ItemId = p.ItemId,
                               ItemName = p.ItemName,
                               CategoryName = _context.ItemCategories
                                                   .Where(pc => pc.CategoryId == p.CategoryId)
                                                   .Select(pc => pc.CategoryName.ToUpper())
                                                   .FirstOrDefault() ?? "",
                               ProductPrice = $"${p.Price}",
                               AvailableStock = i.CurrentStock < 0 ? 0 : i.CurrentStock,
                               MinimumRequiredStock = i.LowStockThreshold
                           };

            return products.ToList();
        }

        // Get the current stock for a given product ID
        public int GetStock(int itemId)
        {
            var stockInfo = _context.Inventories
                                    .Where(i => i.ItemId == itemId)
                                    .Select(i => i.CurrentStock)
                                    .FirstOrDefault();

            return stockInfo < 0 ? 0 : stockInfo;
        }

        // Update the stock for a given product ID
        public bool UpdateStock(int itemId, int currentStock)
        {
            var inventoryItem = _context.Inventories.FirstOrDefault(i => i.ItemId == itemId);

            if (inventoryItem != null)
            {
                inventoryItem.CurrentStock = currentStock;
                _context.SaveChanges();
                return GetStock(itemId) == currentStock;
            }

            return false;
        }

        // Insert a new product into the inventory
        public bool InsertProductToInventory(int itemId, int currentStock, int lowStockThreshold)
        {
            var existingItem = _context.Inventories.FirstOrDefault(i => i.ItemId == itemId);

            if (existingItem == null)
            {
                var newInventoryItem = new Inventory
                {
                    ItemId = itemId,
                    CurrentStock = currentStock,
                    LowStockThreshold = lowStockThreshold
                };

                _context.Inventories.Add(newInventoryItem);
                _context.SaveChanges();
                return GetStock(itemId) == currentStock;
            }

            return false;
        }

        // Decrement the stock for a given Item Id by a specified quantity
        public int DecrementProductStock(int itemId, int quantity)
        {
            var inventoryItem = _context.Inventories.FirstOrDefault(i => i.ItemId == itemId);

            if (inventoryItem != null)
            {
                inventoryItem.CurrentStock -= quantity;
                _context.SaveChanges();
            }

            return GetStock(itemId);
        }
    }
}
