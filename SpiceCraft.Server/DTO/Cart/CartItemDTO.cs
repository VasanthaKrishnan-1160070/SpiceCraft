public class CartItemDTO
{
    public string ItemName { get; set; }
    public string UnitPrice { get; set; }
    public int Quantity { get; set; }
    public string ActualPrice { get; set; }
    public string DiscountRate { get; set; }
    public string FinalPrice { get; set; }
    
    public int CartItemId { get; set; }
    public int CartId { get; set; }
    public int ItemId { get; set; }
    
    public decimal? PriceAtAdd { get; set; }
}