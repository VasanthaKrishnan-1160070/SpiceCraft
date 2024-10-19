using SpiceCraft.Server.DTO.Inventory;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IInventoryRepository
    {
        IEnumerable<ProductInventoryDTO> GetProducts();
        int GetStock(int productId);
        bool UpdateStock(int productId, int currentStock);
        bool InsertProductToInventory(int productId, int currentStock, int lowStockThreshold);
        int DecrementProductStock(int productId, int quantity);
        Task<List<IngredientDTO>> GetLowStockIngredientsAsync();

        Task<IEnumerable<IngredientInventoryDTO>> GetInventory();

        void UpdateInventoryStock(int ingredientId, int newStock);
    }
}
