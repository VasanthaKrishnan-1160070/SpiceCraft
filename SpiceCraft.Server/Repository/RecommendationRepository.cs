using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.ML.Models.Recommendation;
using SpiceCraft.Server.Repository.Interface;
using Microsoft.ML;
using Microsoft.EntityFrameworkCore;


namespace SpiceCraft.Server.Repository;


public class RecommendationRepository : ProductRepository, IRecommendationRepository
{
    private readonly SpiceCraftContext _context;
    private readonly MLContext _mlContext;


    public RecommendationRepository(SpiceCraftContext context): base(context)
    {
        _context = context;
    }
  

    public async Task<List<ProductCatalogItemDTO>> GetRecommenedItems(List<int> recommendedItemIds)
    {
        // Step 2: Check if any recommended items exist for the user
        if (!recommendedItemIds.Any())
        {
            return new List<ProductCatalogItemDTO>(); // Return an empty list if no recommendations exist
        }

        // Step 3: Get the product query
        var productQuery = GetProductQuery();

        // Step 4: Filter the products based on the recommended item IDs
        var recommendedProductQuery = productQuery
            .Where(p => recommendedItemIds.Contains(p.ItemId));

        // Step 5: Execute the query and return the result as a list
        return await recommendedProductQuery.ToListAsync();
    }

    // Helper method to extract training data
    public async Task<List<UserItemData>> ExtractTrainingDataAsync()
    {
        var ratingsData = await _context.UserItemRatings
            .Select(r => new UserItemData
            {
                UserId = (float)r.UserId,
                ItemId = (float)r.ItemId,
                Label = (float)r.Rating
            })
            .ToListAsync();

        var recentlyViewedData = await _context.RecentlyVieweds
            .Select(rv => new UserItemData
            {
                UserId = (float)rv.UserId,
                ItemId = (float)rv.ItemId,
                Label = (float)rv.ViewCount
            })
            .ToListAsync();

        var interactionData = await _context.UserItemInteractions
            .Select(ui => new UserItemData
            {
                UserId = (float)ui.UserId,
                ItemId = 0, // Since this table doesn't have ItemId, we'll treat it differently
                Label = (float)ui.Interaction
            })
            .ToListAsync();

        // Combine all data
        var combinedData = ratingsData.Concat(recentlyViewedData).Concat(interactionData).ToList();

        return combinedData;
    }

    public async Task<List<int>> GetAllItemId()
    {
        return await _context.Items.Select(i => i.ItemId).ToListAsync();
    }
}
