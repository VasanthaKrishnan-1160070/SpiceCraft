using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.RecentlyViewed;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Repository
{
    public class RecentlyViewedItemsRepository : IRecentlyViewedItemsRepository
    {
        private readonly SpiceCraftContext _context;

        public RecentlyViewedItemsRepository(SpiceCraftContext context)
        {
            _context = context;
        }

        public async Task<List<RecentlyViewedItemDTO>> GetRecentlyViewedItemsAsync(int userId)
        {
            return await _context.RecentlyViewed
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new RecentlyViewedItemDTO
                {
                    ItemId = r.ItemId,
                    UserId = r.UserId,
                    ItemName = r.Item.ItemName,
                    ViewedAt = r.CreatedAt
                }).ToListAsync();
        }

        public async Task AddRecentlyViewedItemAsync(RecentlyViewedItemDTO item)
        {
            var entity = new RecentlyViewed
            {
                UserId = item.UserId,
                ItemId = item.ItemId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.RecentlyViewed.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
