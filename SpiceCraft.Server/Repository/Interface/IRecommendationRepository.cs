using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.ML.Models.Recommendation;

namespace SpiceCraft.Server.Repository.Interface;

public interface IRecommendationRepository
{
    Task<List<int>> GetAllItemId();

    Task<List<ProductCatalogItemDTO>> GetRecommenedItems(List<int> recommendedItemIds);

    Task<List<UserItemData>> ExtractTrainingDataAsync();
}