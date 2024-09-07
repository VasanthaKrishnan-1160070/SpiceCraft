namespace SpiceCraft.Server.DTO.Inventory
{
    public class ProductInventoryDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string ProductPrice { get; set; }
        public int AvailableStock { get; set; }
        public int MinimumRequiredStock { get; set; }
    }
}
