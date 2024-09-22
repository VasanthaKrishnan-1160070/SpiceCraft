using SpiceCraft.Server.DTO.Promotions;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IPromotionLogics
    {
        // Get all promotions (items, categories, BOGO, bulk)
        Task<ResultDetail<PromotionDTO>> GetPromotionsAsync();
        
        // Add a new item promotion
        Task<ResultDetail<bool>> AddItemPromotionAsync(ItemPromotionDTO promotion);

        // Add a new category promotion
        Task<ResultDetail<bool>> AddCategoryPromotionAsync(CategoryPromotionDTO promotion);

        // Add a new BOGO promotion
        Task<ResultDetail<bool>> AddBogoPromotionAsync(BogoPromotionDTO promotion);

        // Add a new bulk item promotion
        Task<ResultDetail<bool>> AddBulkItemPromotionAsync(BulkPromotionDTO promotion);

        // Remove an item promotion by item ID
        Task<ResultDetail<bool>> RemoveItemPromotionAsync(int itemId);

        // Remove a category promotion by category ID
        Task<ResultDetail<bool>> RemoveCategoryPromotionAsync(int categoryId);

        // Remove a BOGO promotion by item ID
        Task<ResultDetail<bool>> RemoveBogoPromotionAsync(int itemId);

        // Remove a bulk item promotion by item ID
        Task<ResultDetail<bool>> RemoveBulkItemPromotionAsync(int itemId);

        // Remove all item promotions
        Task<ResultDetail<bool>> RemoveAllItemPromotionsAsync();

        // Remove all category promotions
        Task<ResultDetail<bool>> RemoveAllCategoryPromotionsAsync();

        // Remove all BOGO promotions
        Task<ResultDetail<bool>> RemoveAllBogoPromotionsAsync();

        // Remove all bulk item promotions
        Task<ResultDetail<bool>> RemoveAllBulkItemPromotionsAsync();
    }
}
