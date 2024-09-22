using SpiceCraft.Server.DTO.Promotions;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IPromotionRepository
    {
        // Get all promotions (items, categories, BOGO, bulk)
        PromotionDTO GetPromotions();
        // Add a new item promotion
        bool AddItemPromotion(ItemPromotionDTO promotion);

        // Add a new category promotion
        bool AddCategoryPromotion(CategoryPromotionDTO promotion);

        // Add a new BOGO promotion
        bool AddBogoPromotion(BogoPromotionDTO promotion);

        // Add a new bulk item promotion
        bool AddBulkItemPromotion(BulkPromotionDTO promotion);

        // Remove an item promotion by item ID
        bool RemoveItemPromotion(int itemId);

        // Remove a category promotion by category ID
        bool RemoveCategoryPromotion(int categoryId);

        // Remove a BOGO promotion by item ID
        bool RemoveBogoPromotion(int itemId);

        // Remove a bulk item promotion by item ID
        bool RemoveBulkItemPromotion(int itemId);

        // Remove all item promotions
        bool RemoveAllItemPromotions();

        // Remove all category promotions
        bool RemoveAllCategoryPromotions();

        // Remove all BOGO promotions
        bool RemoveAllBogoPromotions();

        // Remove all bulk item promotions
        bool RemoveAllBulkItemPromotions();
    }
}
