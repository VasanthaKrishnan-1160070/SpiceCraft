using SpiceCraft.Server.DTO.Product;

namespace SpiceCraft.Server.Helpers.Request;

public class CreateUpdateItemRequest
{
    public ProductSummaryDTO ItemSummary { get; set; }
    public ProductImageDto[]? ItemImages { get; set; }
    public IEnumerable<string>? RemovedImages { get; set; }
    public string? MainImageName { get; set; }
}