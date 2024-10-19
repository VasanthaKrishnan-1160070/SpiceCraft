using Microsoft.ML;
using SpiceCraft.Server.ML.Models.Navigation;
using SpiceCraft.Server.ML.Models.Recommendation;

namespace SpiceCraft.Server.ML.Recommendation;

public class RecommendationPredictor
{
    private static MLContext _mlContext = new MLContext();
    private static ITransformer _model;

    public static void LoadModel()
    {
        // Load the trained model
        _model = _mlContext.Model.Load("recommendationModel.zip", out var modelInputSchema);
    }

    public static List<int> PredictRecommendations(List<int> allItems, int userId, int topN)
    {
        // Create a prediction engine
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<UserItemData, UserItemPrediction>(_model);
        var recommendations = new List<Tuple<int, float>>();
        
        // Predict scores for all items for the given user
        foreach (var itemId in allItems)
        {
            var prediction = predictionEngine.Predict(new UserItemData { UserId = userId, ItemId = itemId });
            recommendations.Add(new Tuple<int, float>(itemId, prediction.Score));
        }
        
        // Sort by score and return top N item IDs
        var topRecommendations = recommendations
            .OrderByDescending(r => r.Item2)
            .Take(topN)
            .Select(r => r.Item1)
            .ToList();
        
        return topRecommendations;
    }
}