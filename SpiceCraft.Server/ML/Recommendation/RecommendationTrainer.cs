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

namespace SpiceCraft.Server.ML.Recommendation;

public class RecommendationTrainer
{
    private static string _modelPath = Path.Combine(Environment.CurrentDirectory, "UserNavigationModel.zip");
    private IRecommendationRepository _recommendationRepository;
    private readonly MLContext _mlContext;
    private ITransformer _model;
    
    public RecommendationTrainer(IRecommendationRepository recommendationRepository)
    {
        _recommendationRepository = recommendationRepository;
        _mlContext = new MLContext();
    }
    
    public async Task TrainRecommendationModelAsync()
    {
        // Step 1: Extract data from the tables (RecentlyViewed, UserItemRatings, UserItemInteraction)
        var trainingData = await _recommendationRepository.ExtractTrainingDataAsync();

        // Step 2: Load the data into IDataView for ML.NET
        IDataView data = _mlContext.Data.LoadFromEnumerable(trainingData);

        // Step 3: Define the training pipeline using Matrix Factorization
        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = nameof(UserItemData.UserId),
            MatrixRowIndexColumnName = nameof(UserItemData.ItemId),
            LabelColumnName = nameof(UserItemData.Label),
            NumberOfIterations = 20,
            ApproximationRank = 100
        };

        var pipeline = _mlContext.Recommendation().Trainers.MatrixFactorization(options);

        // Step 4: Train the model
        _model = pipeline.Fit(data);

        // save the trained model to a file:
        _mlContext.Model.Save(_model, data.Schema, "recommendationModel.zip");
    }
}