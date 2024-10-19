namespace SpiceCraft.Server.DTO.Inventory
{
    public class ProductInventoryDTO
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string ProductPrice { get; set; }
        public int AvailableStock { get; set; }
        public int MinimumRequiredStock { get; set; }
        
        public List<IngredientDTO> Ingredients { get; set; }
    }
}
