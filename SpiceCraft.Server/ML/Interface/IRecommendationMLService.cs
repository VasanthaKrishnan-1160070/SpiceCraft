using Microsoft.ML;
using SpiceCraft.Server.ML.Models.Recommendation;

namespace SpiceCraft.Server.ML.Interface;

public interface IRecommendationMLService
{
    ITransformer TrainModel(List<UserItemData> trainingData);
    float Predict(int userId, int itemId);
    void SaveModel(string path);
    void LoadModel(string path);
}