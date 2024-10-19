using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Product;
using SpiceCraft.Server.ML.Models.Recommendation;
using SpiceCraft.Server.Repository.Interface;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpiceCraft.Server.ML.Interface;


namespace SpiceCraft.Server.ML.Recommendation;

public class RecommendationMLService : IRecommendationMLService
{
    private readonly MLContext _mlContext;
    private ITransformer _model;
    private readonly string _modelPath = "recommendation_model.zip"; // Define the path to save/load the model

    public RecommendationMLService()
    {
        _mlContext = new MLContext();
    }

    // Method to train the recommendation model
    public ITransformer TrainModel(List<UserItemData> trainingData)
    {
        IDataView dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

        var pipeline = _mlContext.Recommendation().Trainers.MatrixFactorization(new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = nameof(UserItemData.UserId),
            MatrixRowIndexColumnName = nameof(UserItemData.ItemId),
            LabelColumnName = nameof(UserItemData.Label),
            NumberOfIterations = 20,
            ApproximationRank = 100
        });

        _model = pipeline.Fit(dataView);

        // After training, save the model to a file for future use
        SaveModel(_modelPath);
        return _model;
    }

    // Make predictions for recommended items
    // Predict recommendation score for a user-item pair
    public float Predict(int userId, int itemId)
    {
        // If the model is not loaded yet, load it from the saved file
        if (_model == null)
        {
            LoadModel(_modelPath);
        }

        var predictionEngine = _mlContext.Model.CreatePredictionEngine<UserItemData, UserItemPrediction>(_model);
        var prediction = predictionEngine.Predict(new UserItemData { UserId = userId, ItemId = itemId });
        return prediction.Score;
    }

    // Save the model to disk
    public void SaveModel(string path)
    {
        if (_model != null)
        {
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                _mlContext.Model.Save(_model, null, fileStream);
            }
        }
    }

    // Load the model from disk
    public void LoadModel(string path)
    {
        if (File.Exists(path))
        {
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                _model = _mlContext.Model.Load(fileStream, out var schema);
            }
        }
        else
        {
            throw new FileNotFoundException($"Model file not found at {path}");
        }
    }
}