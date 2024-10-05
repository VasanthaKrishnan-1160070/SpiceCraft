using Microsoft.ML;
using SpiceCraft.Server.Models.ML.Navigation;

namespace SpiceCraft.Server.ML.Navigation;

public class NavigationModelPredictor
{
    private static MLContext _mlContext = new MLContext();
    private static ITransformer _model;

    public static void LoadModel()
    {
        // Load the trained model
        _model = _mlContext.Model.Load("UserNavigationModel.zip", out var modelInputSchema);
    }

    public static float PredictClickLikelihood(UserActivity userActivity)
    {
        // Create a prediction engine
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<UserActivity, NavigationPrediction>(_model);

        // Make a prediction
        var prediction = predictionEngine.Predict(userActivity);
        return prediction.Score; // Score represents the likelihood of clicking
    }
}