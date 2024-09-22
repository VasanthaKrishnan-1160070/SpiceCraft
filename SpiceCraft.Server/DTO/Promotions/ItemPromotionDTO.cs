namespace SpiceCraft.Server.DTO.Promotions;

public class ItemPromotionDTO
{
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public string CategoryName { get; set; }
    public string ActualPrice { get; set; }
    public string DiscountRate { get; set; }
    public string PriceAfterDiscount { get; set; }
    public string HasPromotion { get; set; }
    public string ActionHidden { get; set; }
    public bool ActionAdd { get; set; }
    public bool ActionRemove { get; set; }
}