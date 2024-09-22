namespace SpiceCraft.Server.DTO.Product
{
    public class ProductCatalogItemDTO
    {
        public decimal Price { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsRemoved { get; set; }
        public int CurrentStock { get; set; }
        public bool OwnProduct { get; set; }
        public int CategoryId { get; set; }
        public string ImageCode { get; set; }
        public string CategoryName { get; set; }
        public int ItemId { get; set; }
        public decimal? DiscountRate { get; set; }
        public decimal? BulkDiscountRate { get; set; }
        public int BulkDiscountRequiredQuantity { get; set; }
        public string? ComboName { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string IsInSale { get; set; }
        public string IsLowStock { get; set; }
        public string IsNoStock { get; set; }
    }
}
