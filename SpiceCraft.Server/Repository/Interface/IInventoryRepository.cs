using SpiceCraft.Server.DTO.Inventory;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IInventoryRepository
    {
        IEnumerable<ProductInventoryDTO> GetProducts();
        int GetStock(int productId);
        bool UpdateStock(int productId, int currentStock);
        bool InsertProductToInventory(int productId, int currentStock, int lowStockThreshold);
        int DecrementProductStock(int productId, int quantity);
    }
}
