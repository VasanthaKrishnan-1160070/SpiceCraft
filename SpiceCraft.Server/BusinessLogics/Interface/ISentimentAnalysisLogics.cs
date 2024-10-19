using SpiceCraft.Server.ML.Models.SentitmentAnalysis;

namespace SpiceCraft.Server.BusinessLogics.Interface;

public interface ISentimentAnalysisLogics
{
    Task TrainSentimentModelAsync();
    Task<SentimentPrediction> AnalyzeRatingDescriptionAsync(string ratingDescription);
}