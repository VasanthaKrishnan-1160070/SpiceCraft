using Microsoft.ML;
using SpiceCraft.Server.ML.Models.SentitmentAnalysis;

namespace SpiceCraft.Server.ML.Interface;

public interface ISentimentMLService
{
    ITransformer TrainModel(List<RatingDescriptionData> data);
    SentimentPrediction Predict(string ratingDescription);
    void SaveModel(string path);
    void LoadModel(string path);
}