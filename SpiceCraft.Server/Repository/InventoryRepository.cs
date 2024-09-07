using SpiceCraft.Server.Context;
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

        public int GetStock(int productId)
        {
            // Get the current stock for a given product ID
            var stockInfo = _context.Inventories
                                    .Where(i => i.ItemId == productId)
                                    .Select(i => i.CurrentStock)
                                    .FirstOrDefault();

            // Return the current stock or 0 if not found
            return stockInfo < 0 ? 0 : stockInfo;
        }

        public bool UpdateStock(int productId, int currentStock)
        {
            // Update the stock for a given product ID
            var inventoryItem = _context.Inventories.FirstOrDefault(i => i.ItemId == productId);

            if (inventoryItem != null)
            {
                inventoryItem.CurrentStock = currentStock;
                _context.SaveChanges();
            }

            // Verify if the stock update was successful
            return GetStock(productId) == currentStock;
        }

        public bool InsertProductToInventory(int productId, int currentStock, int lowStockThreshold)
        {
            // Check if the product already exists in the inventory
            var existingItem = _context.Inventories.FirstOrDefault(i => i.ItemId == productId);

            if (existingItem == null)
            {
                // Insert a new product into the inventory
                var newInventoryItem = new Inventory
                {
                    ItemId = productId,
                    CurrentStock = currentStock,
                    LowStockThreshold = lowStockThreshold
                };

                _context.Inventories.Add(newInventoryItem);
                _context.SaveChanges();
            }

            // Verify if the product was successfully inserted
            return GetStock(productId) == currentStock;
        }

        public int DecrementProductStock(int productId, int quantity)
        {
            // Decrement the stock for a given product ID by a specified quantity
            var inventoryItem = _context.Inventories.FirstOrDefault(i => i.ItemId == productId);

            if (inventoryItem != null)
            {
                inventoryItem.CurrentStock -= quantity;
                _context.SaveChanges();
            }

            // Return the updated stock
            return GetStock(productId);
        }
    }
}
