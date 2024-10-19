using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.DTO.RecentlyViewed;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Repository
{
    public class RecentlyViewedItemsRepository : ProductRepository, IRecentlyViewedItemsRepository
    {
        private readonly SpiceCraftContext _context;

        public RecentlyViewedItemsRepository(SpiceCraftContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProductCatalogItemDTO>> GetRecentlyViewedItemsAsync(int userId, int maxCount = 5)
        {
            // Step 1: Retrieve all distinct recently viewed items for the specified user
            var recentlyViewedItems = await _context.RecentlyVieweds
                .Where(rv => rv.UserId == userId)
                .Select(rv => rv.ItemId)
                .Distinct() // Ensure only unique ItemId's are selected
                .ToListAsync();

            // Step 2: Check if any recently viewed items exist for the user
            if (!recentlyViewedItems.Any())
            {
                return new List<ProductCatalogItemDTO>(); // Return an empty list if no recently viewed items exist
            }

            // Step 3: Get the product query
            var productQuery = GetProductQuery();

            // Step 4: Filter the products based on the distinct recently viewed items
            var recentlyViewedProductQuery = productQuery
                .Where(p => recentlyViewedItems.Contains(p.ItemId));
                

            // Step 5: Execute the query and return the result as a list
            return await recentlyViewedProductQuery.Take(maxCount).ToListAsync();
        }



        public async Task AddRecentlyViewedItemAsync(RecentlyViewedItemDTO item)
        {
            // Check if the item has already been viewed by the user
            var existingEntity = await _context.RecentlyVieweds
                .FirstOrDefaultAsync(rv => rv.UserId == item.UserId && rv.ItemId == item.ItemId);

            if (existingEntity != null)
            {
                // Item already viewed, update the ViewCount and UpdatedAt
                existingEntity.ViewCount += 1;
                existingEntity.UpdatedAt = DateTime.Now;
            }
            else
            {
                // Item not viewed yet, create a new record
                var newEntity = new RecentlyViewed
                {
                    UserId = item.UserId,
                    ItemId = item.ItemId,
                    ViewCount = 1,  // Initialize ViewCount for the new entry
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                _context.RecentlyVieweds.Add(newEntity);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

    }
}
