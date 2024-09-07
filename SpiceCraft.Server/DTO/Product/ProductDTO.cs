namespace SpiceCraft.Server.DTO.Product;

public class ProductDTO
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal Price { get; set; }
    public bool OwnProduct { get; set; }
    public bool IsRemoved { get; set; }
    public int CurrentStock { get; set; }
    public string ImageCode { get; set; }
    public int CategoryId { get; set; }
    public string CategoryName { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal BulkDiscountRate { get; set; }
    public int BulkDiscountRequiredQuantity { get; set; }
    public string ComboName { get; set; }
    public decimal DiscountPrice { get; set; }
    public string IsInSale { get; set; }
    public string IsLowStock { get; set; }
    public string IsNoStock { get; set; }
    public bool IsMain { get; set; }
}
