using SpiceCraft.Server.DTO.RecentlyViewed;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IRecentlyViewedItemsRepository
    {
        Task<List<RecentlyViewedItemDTO>> GetRecentlyViewedItemsAsync(int userId);
        Task AddRecentlyViewedItemAsync(RecentlyViewedItemDTO item);
    }
}
