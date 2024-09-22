namespace SpiceCraft.Server.DTO.Promotions;

public class CategoryPromotionDTO
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string ParentCategoryName { get; set; }
    public string DiscountRate { get; set; }
    public string HasPromotion { get; set; }
    public string ActionHidden { get; set; }
    public bool ActionAdd { get; set; }
    public bool ActionRemove { get; set; }
}