using Microsoft.ML;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Data;
using System.Collections.Generic;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SpiceCraft.Server.ML.Models.Navigation;

namespace SpiceCraft.Server.ML.Navigation;

public class NavigationModelTrainer
{
    private static string _modelPath = Path.Combine(Environment.CurrentDirectory, "UserNavigationModel.zip");
    private static MLContext _mlContext = new MLContext();

    public static void TrainAndEvaluateModel()
{
    // Load training and test data
    IDataView trainingData = _mlContext.Data.LoadFromEnumerable(GetBalancedMockTrainingData());
    IDataView testData = _mlContext.Data.LoadFromEnumerable(GetMockTestData());

    // Data processing pipeline
    var dataProcessPipeline = _mlContext.Transforms.Concatenate("Features", new[] { "ClickCount", "TimeSpent", "LastClicked", "SessionOrder" })
        .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
        .Append(_mlContext.BinaryClassification.Trainers.FastTree(
            labelColumnName: "Clicked",      // Specify the correct label column
            featureColumnName: "Features",
            numberOfLeaves: 20,              
            minimumExampleCountPerLeaf: 10,  
            numberOfTrees: 100,              
            learningRate: 0.1));

    // Train the model
    var model = dataProcessPipeline.Fit(trainingData);

    // Evaluate the model
    var predictions = model.Transform(testData);
    var metrics = _mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: "Clicked");

    // Only display AUC if both classes are present
    if (metrics.AreaUnderRocCurve != 0 && !double.IsNaN(metrics.AreaUnderRocCurve))
    {
        Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:P2}");
    }
    else
    {
        Console.WriteLine("AUC cannot be calculated due to lack of negative class in the data.");
    }

    // Display other evaluation metrics
    Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");

    // Convert likelihoods to probabilities and display them
    var predictionEngine = _mlContext.Model.CreatePredictionEngine<UserActivity, UserActivityPrediction>(model);
    foreach (var testItem in GetMockTestData())
    {
        var prediction = predictionEngine.Predict(testItem);
        double probability = Sigmoid(prediction.Likelihood);
        Console.WriteLine($"Navigation Item: {testItem.NavigationItem}, Likelihood: {prediction.Likelihood}, Probability: {probability:P2}");
    }

    // Perform cross-validation
    var crossValidationResults = _mlContext.BinaryClassification.CrossValidate(
        trainingData, dataProcessPipeline, numberOfFolds: 5, labelColumnName: "Clicked");

    foreach (var result in crossValidationResults)
    {
        Console.WriteLine($"Fold: {result.Fold}, Accuracy: {result.Metrics.Accuracy:P2}, AUC: {result.Metrics.AreaUnderRocCurve:P2}");
    }

    // Save the model
    _mlContext.Model.Save(model, trainingData.Schema, "UserNavigationModel.zip");
}

// Sigmoid function to convert raw scores to probabilities
public static double Sigmoid(double rawScore)
{
    return 1 / (1 + Math.Exp(-rawScore));
}


    private static void EvaluateModel(ITransformer model, IDataView testData)
    {
        var predictions = model.Transform(testData);
        var metrics = _mlContext.BinaryClassification.Evaluate(predictions, labelColumnName: "Clicked");

        // Print evaluation metrics
        Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
        Console.WriteLine($"AUC: {metrics.AreaUnderRocCurve:P2}");
        Console.WriteLine($"F1 Score: {metrics.F1Score:P2}");
        Console.WriteLine($"Precision: {metrics.PositivePrecision:P2}");
        Console.WriteLine($"Recall: {metrics.PositiveRecall:P2}");
    }

    private static void PerformCrossValidation(IDataView trainingData, IEstimator<ITransformer> dataProcessPipeline)
    {
        var crossValidationResults = _mlContext.BinaryClassification.CrossValidate(trainingData, dataProcessPipeline, numberOfFolds: 5, labelColumnName: "Clicked");

        // Display cross-validation results
        foreach (var result in crossValidationResults)
        {
            Console.WriteLine($"Fold: {result.Fold}, Accuracy: {result.Metrics.Accuracy:P2}, AUC: {result.Metrics.AreaUnderRocCurve:P2}");
        }
    }


    public static List<UserActivity> GetBalancedMockTrainingData()
    {
        return new List<UserActivity>
        {
            // Positive examples (Clicked = true)
            new UserActivity
            {
                UserId = 1, NavigationItem = "Home", ClickCount = 5, TimeSpent = 120, LastClicked = 300,
                SessionOrder = 1, Clicked = true
            },
            new UserActivity
            {
                UserId = 1, NavigationItem = "Dashboard", ClickCount = 3, TimeSpent = 90, LastClicked = 600,
                SessionOrder = 2, Clicked = true
            },
            new UserActivity
            {
                UserId = 1, NavigationItem = "Orders", ClickCount = 4, TimeSpent = 100, LastClicked = 400,
                SessionOrder = 3, Clicked = true
            },
            new UserActivity
            {
                UserId = 1, NavigationItem = "Cart", ClickCount = 2, TimeSpent = 80, LastClicked = 500,
                SessionOrder = 4, Clicked = true
            },
            new UserActivity
            {
                UserId = 1, NavigationItem = "Payments", ClickCount = 3, TimeSpent = 70, LastClicked = 300,
                SessionOrder = 5, Clicked = true
            },
            new UserActivity
            {
                UserId = 2, NavigationItem = "Home", ClickCount = 6, TimeSpent = 140, LastClicked = 200,
                SessionOrder = 1, Clicked = true
            },
            new UserActivity
            {
                UserId = 2, NavigationItem = "Dashboard", ClickCount = 5, TimeSpent = 130, LastClicked = 400,
                SessionOrder = 2, Clicked = true
            },
            new UserActivity
            {
                UserId = 2, NavigationItem = "Profile", ClickCount = 2, TimeSpent = 110, LastClicked = 700,
                SessionOrder = 3, Clicked = true
            },
            new UserActivity
            {
                UserId = 3, NavigationItem = "Menu", ClickCount = 4, TimeSpent = 100, LastClicked = 500,
                SessionOrder = 1, Clicked = true
            },
            new UserActivity
            {
                UserId = 3, NavigationItem = "Enquiry", ClickCount = 3, TimeSpent = 90, LastClicked = 400,
                SessionOrder = 2, Clicked = true
            },
            new UserActivity
            {
                UserId = 3, NavigationItem = "Returns", ClickCount = 6, TimeSpent = 60, LastClicked = 800,
                SessionOrder = 3, Clicked = true
            },
            new UserActivity
            {
                UserId = 4, NavigationItem = "Home", ClickCount = 7, TimeSpent = 150, LastClicked = 300,
                SessionOrder = 1, Clicked = true
            },
            new UserActivity
            {
                UserId = 4, NavigationItem = "Dashboard", ClickCount = 8, TimeSpent = 100, LastClicked = 600,
                SessionOrder = 2, Clicked = true
            },
            new UserActivity
            {
                UserId = 5, NavigationItem = "Profile", ClickCount = 5, TimeSpent = 70, LastClicked = 500,
                SessionOrder = 1, Clicked = true
            },
            new UserActivity
            {
                UserId = 5, NavigationItem = "Payments", ClickCount = 4, TimeSpent = 60, LastClicked = 300,
                SessionOrder = 2, Clicked = true
            },
            new UserActivity
            {
                UserId = 5, NavigationItem = "Cart", ClickCount = 2, TimeSpent = 50, LastClicked = 700,
                SessionOrder = 3, Clicked = true
            },
            new UserActivity
            {
                UserId = 6, NavigationItem = "Orders", ClickCount = 6, TimeSpent = 120, LastClicked = 400,
                SessionOrder = 1, Clicked = true
            },
            new UserActivity
            {
                UserId = 6, NavigationItem = "Profile", ClickCount = 3, TimeSpent = 90, LastClicked = 600,
                SessionOrder = 2, Clicked = true
            },
            new UserActivity
            {
                UserId = 6, NavigationItem = "Payments", ClickCount = 5, TimeSpent = 130, LastClicked = 500,
                SessionOrder = 3, Clicked = true
            },

            // Negative examples (Clicked = false)
            new UserActivity
            {
                UserId = 1, NavigationItem = "Home", ClickCount = 0, TimeSpent = 30, LastClicked = 1000,
                SessionOrder = 1, Clicked = false
            },
            new UserActivity
            {
                UserId = 1, NavigationItem = "Profile", ClickCount = 1, TimeSpent = 60, LastClicked = 1200,
                SessionOrder = 2, Clicked = false
            },
            new UserActivity
            {
                UserId = 1, NavigationItem = "Cart", ClickCount = 0, TimeSpent = 50, LastClicked = 1100,
                SessionOrder = 3, Clicked = false
            },
            new UserActivity
            {
                UserId = 2, NavigationItem = "Enquiry", ClickCount = 1, TimeSpent = 40, LastClicked = 900,
                SessionOrder = 1, Clicked = false
            },
            new UserActivity
            {
                UserId = 2, NavigationItem = "Returns", ClickCount = 0, TimeSpent = 20, LastClicked = 1000,
                SessionOrder = 2, Clicked = false
            },
            new UserActivity
            {
                UserId = 3, NavigationItem = "Payments", ClickCount = 0, TimeSpent = 60, LastClicked = 1300,
                SessionOrder = 1, Clicked = false
            },
            new UserActivity
            {
                UserId = 3, NavigationItem = "Orders", ClickCount = 2, TimeSpent = 100, LastClicked = 1200,
                SessionOrder = 2, Clicked = false
            },
            new UserActivity
            {
                UserId = 3, NavigationItem = "Profile", ClickCount = 1, TimeSpent = 40, LastClicked = 800,
                SessionOrder = 3, Clicked = false
            },
            new UserActivity
            {
                UserId = 4, NavigationItem = "Dashboard", ClickCount = 0, TimeSpent = 20, LastClicked = 1400,
                SessionOrder = 1, Clicked = false
            },
            new UserActivity
            {
                UserId = 4, NavigationItem = "Home", ClickCount = 1, TimeSpent = 50, LastClicked = 1000,
                SessionOrder = 2, Clicked = false
            },
            new UserActivity
            {
                UserId = 4, NavigationItem = "Orders", ClickCount = 0, TimeSpent = 40, LastClicked = 1500,
                SessionOrder = 3, Clicked = false
            },
            new UserActivity
            {
                UserId = 5, NavigationItem = "Payments", ClickCount = 1, TimeSpent = 30, LastClicked = 1600,
                SessionOrder = 1, Clicked = false
            },
            new UserActivity
            {
                UserId = 5, NavigationItem = "Cart", ClickCount = 0, TimeSpent = 50, LastClicked = 1100,
                SessionOrder = 2, Clicked = false
            },
            new UserActivity
            {
                UserId = 6, NavigationItem = "Home", ClickCount = 2, TimeSpent = 70, LastClicked = 1200,
                SessionOrder = 1, Clicked = false
            },
            new UserActivity
            {
                UserId = 6, NavigationItem = "Profile", ClickCount = 1, TimeSpent = 50, LastClicked = 1300,
                SessionOrder = 2, Clicked = false
            },
            new UserActivity
            {
                UserId = 6, NavigationItem = "Dashboard", ClickCount = 0, TimeSpent = 40, LastClicked = 1500,
                SessionOrder = 3, Clicked = false
            }
        };
    }

    public static List<UserActivity> GetMockTestData()
    {
        return new List<UserActivity>
        {
            // Some positive (true) examples
            new UserActivity { UserId = 1, NavigationItem = "Home", ClickCount = 5, TimeSpent = 120, LastClicked = 300, SessionOrder = 1, Clicked = true },
            new UserActivity { UserId = 1, NavigationItem = "Dashboard", ClickCount = 3, TimeSpent = 90, LastClicked = 600, SessionOrder = 2, Clicked = true },
            new UserActivity { UserId = 1, NavigationItem = "Profile", ClickCount = 1, TimeSpent = 60, LastClicked = 1200, SessionOrder = 3, Clicked = true },
        
            // Some negative (false) examples
            new UserActivity { UserId = 2, NavigationItem = "Orders", ClickCount = 0, TimeSpent = 0, LastClicked = 1000, SessionOrder = 1, Clicked = false },
            new UserActivity { UserId = 2, NavigationItem = "Cart", ClickCount = 1, TimeSpent = 30, LastClicked = 800, SessionOrder = 2, Clicked = false },
            new UserActivity { UserId = 2, NavigationItem = "Payments", ClickCount = 0, TimeSpent = 20, LastClicked = 1200, SessionOrder = 3, Clicked = false }
        };
    }
}

// Define the UserActivityPrediction class to hold prediction results
public class UserActivityPrediction
{
    [ColumnName("Score")]
    public float Likelihood { get; set; }
}