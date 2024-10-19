using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.DTO.RecentlyViewed;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IRecentlyViewedItemsRepository
    {
        Task<List<ProductCatalogItemDTO>> GetRecentlyViewedItemsAsync(int userId, int maxCount = 5);
        Task AddRecentlyViewedItemAsync(RecentlyViewedItemDTO item);
    }
}
