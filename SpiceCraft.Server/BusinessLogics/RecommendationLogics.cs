using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.ML.Interface;
using SpiceCraft.Server.ML.Recommendation;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics;

public class RecommendationLogics : IRecommendationLogics
{
    private readonly IRecommendationRepository _recommendationRepository;
    private readonly IRecommendationMLService _mlService;
    
    // Train the recommendation model
    public async Task TrainRecommendationModelAsync()
    {
        var trainingData = await _recommendationRepository.ExtractTrainingDataAsync();
        _mlService.TrainModel(trainingData);

        // Optionally, save the model
        _mlService.SaveModel("recommendation_model.zip");
    }

    public RecommendationLogics(IRecommendationRepository recommendationRepository, IRecommendationMLService mlService)
    {
        _recommendationRepository = recommendationRepository;
        _mlService = mlService;
    }

// Get top N recommended items for a user
    public async Task<List<ProductCatalogItemDTO>> GetRecommendedItemsAsync(int userId, int topN)
    {
        // Get recommended item IDs from the ML model
        var allItems = await _recommendationRepository.GetAllItemId();
        var recommendations = new List<Tuple<int, float>>();

        foreach (var itemId in allItems)
        {
            var score = _mlService.Predict(userId, itemId);
            recommendations.Add(new Tuple<int, float>(itemId, score));
        }

        // Sort by score and take top N
        var topRecommendations = recommendations.OrderByDescending(r => r.Item2).Take(topN).Select(r => r.Item1).ToList();

        // Fetch product details for the recommended items
       return await _recommendationRepository.GetRecommenedItems(topRecommendations);
       
    }
}
