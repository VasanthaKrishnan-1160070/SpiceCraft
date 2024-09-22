namespace SpiceCraft.Server.DTO.Order;

public class OrderDetailDTO
{
    public int OrderId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal PurchasePrice { get; set; }
    public string ItemName { get; set; }
    public decimal Price { get; set; }
}