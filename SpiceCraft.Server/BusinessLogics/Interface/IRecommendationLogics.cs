using SpiceCraft.Server.DTO.Product;

namespace SpiceCraft.Server.BusinessLogics.Interface;

public interface IRecommendationLogics
{
    Task TrainRecommendationModelAsync();
    Task<List<ProductCatalogItemDTO>> GetRecommendedItemsAsync(int userId, int topN);
}