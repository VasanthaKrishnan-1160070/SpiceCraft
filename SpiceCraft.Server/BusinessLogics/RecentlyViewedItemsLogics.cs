using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.DTO.RecentlyViewed;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics
{
    public class RecentlyViewedItemsLogics : IRecentlyViewedItemsLogics
    {
        private readonly IRecentlyViewedItemsRepository _recentlyViewedItemsRepository;

        public RecentlyViewedItemsLogics(IRecentlyViewedItemsRepository recentlyViewedItemsRepository)
        {
            _recentlyViewedItemsRepository = recentlyViewedItemsRepository;
        }

        public async Task<ResultDetail<List<ProductCatalogItemDTO>>> GetRecentlyViewedItemsAsync(int userId, int maxCount = 5)
        {
            var result = await _recentlyViewedItemsRepository.GetRecentlyViewedItemsAsync(userId, maxCount);
            return result == null || result.Count == 0
                ? HelperFactory.Msg.Error<List<ProductCatalogItemDTO>>("No recently viewed items found")
                : HelperFactory.Msg.Success(result);
        }

        public async Task<ResultDetail<bool>> AddRecentlyViewedItemAsync(RecentlyViewedItemDTO item)
        {
            await _recentlyViewedItemsRepository.AddRecentlyViewedItemAsync(item);
            return HelperFactory.Msg.Success(true);
        }
    }
}
