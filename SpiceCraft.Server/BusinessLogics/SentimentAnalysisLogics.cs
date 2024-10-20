using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.ML.Interface;
using SpiceCraft.Server.ML.Models.SentitmentAnalysis;
using SpiceCraft.Server.ML.RatingSentiment;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics;

public class SentimentAnalysisLogics : ISentimentAnalysisLogics
{
    private readonly IUserItemRatingRepository _sentimentRepository;
    private readonly ISentimentMLService _mlService;

    public SentimentAnalysisLogics(IUserItemRatingRepository sentimentRepository, ISentimentMLService mlService)
    {
        _sentimentRepository = sentimentRepository;
        _mlService = mlService;
    }

    // Train the sentiment analysis model
    public async Task TrainSentimentModelAsync()
    {
        var trainingData = SentitmentTrainingData.GetTrainingData();
        _mlService.TrainModel(trainingData);
    }

    // Analyze the sentiment of a given rating description
    public async Task<SentimentPrediction> AnalyzeRatingDescriptionAsync(string ratingDescription)
    {
        return await Task.Run(() => _mlService.Predict(ratingDescription));
    }
}