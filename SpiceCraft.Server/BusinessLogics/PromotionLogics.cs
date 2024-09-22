using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Promotions;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{
public class PromotionLogics : IPromotionLogics
{
    private readonly IPromotionRepository _promotionRepository;

    public PromotionLogics(IPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    // Get all promotions (products, categories, BOGO, bulk)
    public async Task<ResultDetail<PromotionDTO>> GetPromotionsAsync()
    {
        var promotions = _promotionRepository.GetPromotions();
        if (promotions == null)
        {
            return HelperFactory.Msg.Error<PromotionDTO>("No promotions found.");
        }

        return HelperFactory.Msg.Success(promotions, "Promotions retrieved successfully.");
    }
    
    // Add a new item promotion
    public async Task<ResultDetail<bool>> AddItemPromotionAsync(ItemPromotionDTO promotion)
    {
        var result = _promotionRepository.AddItemPromotion(promotion);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to add item promotion.");
        }

        return HelperFactory.Msg.Success(true, "Item promotion added successfully.");
    }

    // Add a new category promotion
    public async Task<ResultDetail<bool>> AddCategoryPromotionAsync(CategoryPromotionDTO promotion)
    {
        var result = _promotionRepository.AddCategoryPromotion(promotion);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to add category promotion.");
        }

        return HelperFactory.Msg.Success(true, "Category promotion added successfully.");
    }

    // Add a new BOGO promotion
    public async Task<ResultDetail<bool>> AddBogoPromotionAsync(BogoPromotionDTO promotion)
    {
        var result = _promotionRepository.AddBogoPromotion(promotion);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to add BOGO promotion.");
        }

        return HelperFactory.Msg.Success(true, "BOGO promotion added successfully.");
    }

    // Add a new bulk item promotion
    public async Task<ResultDetail<bool>> AddBulkItemPromotionAsync(BulkPromotionDTO promotion)
    {
        var result = _promotionRepository.AddBulkItemPromotion(promotion);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to add bulk item promotion.");
        }

        return HelperFactory.Msg.Success(true, "Bulk item promotion added successfully.");
    }

    // Remove a product promotion by product ID
    public async Task<ResultDetail<bool>> RemoveItemPromotionAsync(int itemId)
    {
        var result = _promotionRepository.RemoveItemPromotion(itemId);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to remove product promotion.");
        }

        return HelperFactory.Msg.Success(true, "Item promotion removed successfully.");
    }

    // Remove a category promotion by category ID
    public async Task<ResultDetail<bool>> RemoveCategoryPromotionAsync(int categoryId)
    {
        var result = _promotionRepository.RemoveCategoryPromotion(categoryId);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to remove category promotion.");
        }

        return HelperFactory.Msg.Success(true, "Category promotion removed successfully.");
    }

    // Remove a BOGO promotion by product ID
    public async Task<ResultDetail<bool>> RemoveBogoPromotionAsync(int itemId)
    {
        var result = _promotionRepository.RemoveBogoPromotion(itemId);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to remove BOGO promotion.");
        }

        return HelperFactory.Msg.Success(true, "BOGO promotion removed successfully.");
    }

    // Remove a bulk product promotion by product ID
    public async Task<ResultDetail<bool>> RemoveBulkItemPromotionAsync(int itemId)
    {
        var result = _promotionRepository.RemoveBulkItemPromotion(itemId);
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to remove bulk product promotion.");
        }

        return HelperFactory.Msg.Success(true, "Bulk product promotion removed successfully.");
    }

    // Remove all product promotions
    public async Task<ResultDetail<bool>> RemoveAllItemPromotionsAsync()
    {
        var result = _promotionRepository.RemoveAllItemPromotions();
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to remove all product promotions.");
        }

        return HelperFactory.Msg.Success(true, "All product promotions removed successfully.");
    }

    // Remove all category promotions
    public async Task<ResultDetail<bool>> RemoveAllCategoryPromotionsAsync()
    {
        var result = _promotionRepository.RemoveAllCategoryPromotions();
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to remove all category promotions.");
        }

        return HelperFactory.Msg.Success(true, "All category promotions removed successfully.");
    }

    // Remove all BOGO promotions
    public async Task<ResultDetail<bool>> RemoveAllBogoPromotionsAsync()
    {
        var result = _promotionRepository.RemoveAllBogoPromotions();
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to remove all BOGO promotions.");
        }

        return HelperFactory.Msg.Success(true, "All BOGO promotions removed successfully.");
    }

    // Remove all bulk product promotions
    public async Task<ResultDetail<bool>> RemoveAllBulkItemPromotionsAsync()
    {
        var result = _promotionRepository.RemoveAllBulkItemPromotions();
        if (!result)
        {
            return HelperFactory.Msg.Error<bool>("Failed to remove all bulk product promotions.");
        }

        return HelperFactory.Msg.Success(true, "All bulk product promotions removed successfully.");
    }
}
}
