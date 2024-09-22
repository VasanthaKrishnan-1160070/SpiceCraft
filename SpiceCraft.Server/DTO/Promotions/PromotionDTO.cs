namespace SpiceCraft.Server.DTO.Promotions;

public class PromotionDTO
{
    public List<ItemPromotionDTO> Items { get; set; }
    public List<CategoryPromotionDTO> Categories { get; set; }
    public List<BogoPromotionDTO> BogoPromotions { get; set; }
    public List<BulkPromotionDTO> BulkPromotions { get; set; }
}