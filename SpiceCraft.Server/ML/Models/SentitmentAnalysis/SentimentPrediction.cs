namespace SpiceCraft.Server.ML.Models.SentitmentAnalysis;

public class SentimentPrediction
{
    public bool PredictedLabel { get; set; }
    public float Probability { get; set; }
    public float Score { get; set; }
}