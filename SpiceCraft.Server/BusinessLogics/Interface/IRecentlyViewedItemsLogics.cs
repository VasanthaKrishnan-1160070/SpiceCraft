using SpiceCraft.Server.DTO.RecentlyViewed;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IRecentlyViewedItemsLogics
    {
        Task<ResultDetail<List<RecentlyViewedItemDTO>>> GetRecentlyViewedItemsAsync(int userId);
        Task<ResultDetail<bool>> AddRecentlyViewedItemAsync(RecentlyViewedItemDTO item);
    }
}
