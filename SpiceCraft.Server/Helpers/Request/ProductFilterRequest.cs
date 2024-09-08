using SpiceCraft.Server.Enum;

namespace SpiceCraft.Server.Helpers.Request;

public class ProductFilterRequest
{
    public int CategoryId { get; set; }
    public int SubCategoryId { get; set; }
    public string? Keyword { get; set; }
    public ProductFilterEnum Filter { get; set; }
    public ProductSortingEnum Sorting { get; set; }
    public bool IncludeRemovedProducts { get; set; }
}