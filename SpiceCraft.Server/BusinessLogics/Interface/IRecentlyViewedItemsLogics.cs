using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.DTO.RecentlyViewed;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IRecentlyViewedItemsLogics
    {
        Task<ResultDetail<List<ProductCatalogItemDTO>>> GetRecentlyViewedItemsAsync(int userId, int maxCount = 5);
        Task<ResultDetail<bool>> AddRecentlyViewedItemAsync(RecentlyViewedItemDTO item);
    }
}
