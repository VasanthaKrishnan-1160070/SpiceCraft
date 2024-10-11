namespace SpiceCraft.Server.DTO.Product;

public class ItemImagesDTO
{
    public int ItemId { get; set; }
    public IEnumerable<string> ImageNames { get; set; }
    public string MainImageName { get; set; }
}