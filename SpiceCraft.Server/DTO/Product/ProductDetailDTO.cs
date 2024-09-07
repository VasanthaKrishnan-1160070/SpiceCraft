using SpiceCraft.Server.DTO.Category;
using SpiceCraft.Server.DTO.ItemImage;

namespace SpiceCraft.Server.DTO.Product;

public class ProductDetailDTO
{
    public ProductSummaryDTO? ProductDetails { get; set; }
    public IEnumerable<CategoryDTO> Categories { get; set; }
    public IEnumerable<CategoryDTO> SubCategories { get; set; }
    public IEnumerable<ItemImageDTO> ProductImages { get; set; }
}
