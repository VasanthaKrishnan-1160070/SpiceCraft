namespace SpiceCraft.Server.DTO.Product
{
    public class ProductSummaryDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        
        public string MainImageCode { get; set; }
        public DateTime? CreatedAt { get; set; }
        public decimal Price { get; set; }
        public bool OwnProduct { get; set; }
        public bool IsRemoved { get; set; }
        public string CategoryName { get; set; }
        public int? ParentCategoryId { get; set; } // Nullable if there's no parent category
        public int SubCategoryId { get; set; }
        public decimal DiscountRate { get; set; }
        public int? CurrentStock { get; set; } // Nullable if inventory is not found
        public bool? IsMain { get; set; } // Nullable if the product has no main image
        
    }
}
