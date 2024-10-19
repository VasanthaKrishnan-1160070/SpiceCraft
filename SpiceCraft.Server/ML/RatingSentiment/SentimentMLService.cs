using Microsoft.ML;
using SpiceCraft.Server.ML.Interface;
using SpiceCraft.Server.ML.Models.SentitmentAnalysis;

namespace SpiceCraft.Server.ML.RatingSentiment;

public class SentimentMLService : ISentimentMLService
{
    private readonly MLContext _mlContext;
    private ITransformer _model;
    private readonly string _modelPath = "sentiment_model.zip";  // Path for saving/loading model

    public SentimentMLService()
    {
        _mlContext = new MLContext();
    }

    // Train the sentiment analysis model
    public ITransformer TrainModel(List<RatingDescriptionData> data)
    {
        IDataView dataView = _mlContext.Data.LoadFromEnumerable(data);

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(RatingDescriptionData.RatingDescription))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: nameof(RatingDescriptionData.Label), featureColumnName: "Features"));

        _model = pipeline.Fit(dataView);

        // Save the model after training
        SaveModel(_modelPath);
        return _model;
    }

    // Predict sentiment for a rating description
    public SentimentPrediction Predict(string ratingDescription)
    {
        if (_model == null)
        {
            LoadModel(_modelPath);
        }

        var predictionEngine = _mlContext.Model.CreatePredictionEngine<RatingDescriptionData, SentimentPrediction>(_model);
        var prediction = predictionEngine.Predict(new RatingDescriptionData { RatingDescription = ratingDescription });
        return prediction;
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