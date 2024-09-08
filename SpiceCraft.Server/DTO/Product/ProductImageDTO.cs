namespace SpiceCraft.Server.DTO.Product;

public class ProductImageDto
{
    public int ItemId { get; set; }
    public string ImageCode { get; set; }
    public string ImageName { get; set; }
    public int ImageIndex { get; set; }
    public bool IsMain { get; set; }
}
