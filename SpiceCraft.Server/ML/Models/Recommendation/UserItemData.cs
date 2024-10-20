namespace SpiceCraft.Server.ML.Models.Recommendation;

public class UserItemData
{
    public float UserId { get; set; }
    public float ItemId { get; set; }
    public float Label { get; set; }   // Rating, Interaction, or ViewCount
}

public class UserItemPrediction
{
    public float Score { get; set; }
}