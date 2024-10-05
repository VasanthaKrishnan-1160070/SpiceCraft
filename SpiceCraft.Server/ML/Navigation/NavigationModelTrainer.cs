using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.IO;
using SpiceCraft.Server.Models.ML.Navigation;

namespace SpiceCraft.Server.ML.Navigation;

public class NavigationModelTrainer
{
    private static string _modelPath = Path.Combine(Environment.CurrentDirectory, "UserNavigationModel.zip");
    private static MLContext _mlContext = new MLContext();

    public static void TrainModel(IEnumerable<UserActivity> userActivityData)
    {
        // Load training data from the provided user activity data
        IDataView dataView = _mlContext.Data.LoadFromEnumerable(userActivityData);

        // Define the training pipeline
        var dataProcessPipeline = _mlContext.Transforms.Concatenate("Features", new[] { "ClickCount", "TimeSpent", "LastClicked", "SessionOrder" })
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.BinaryClassification.Trainers.LbfgsLogisticRegression(labelColumnName: "Clicked", featureColumnName: "Features"));

        // Train the model
        var model = dataProcessPipeline.Fit(dataView);

        // Save the trained model
        _mlContext.Model.Save(model, dataView.Schema, _modelPath);

        Console.WriteLine($"Model saved to {_modelPath}");
    }
}