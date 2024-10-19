export interface SentimentPredictionModel {
  predictedLabel: boolean;
  probability: number;
  score: number;
}
