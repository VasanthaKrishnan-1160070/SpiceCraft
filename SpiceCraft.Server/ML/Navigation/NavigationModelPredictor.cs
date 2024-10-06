using Microsoft.ML;
using Microsoft.ML.Data;
using SpiceCraft.Server.ML.Models.Navigation;

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

public class NavigationPrediction
{
    [ColumnName("Score")]
    public float Score { get; set; } // The predicted score (click probability)
}