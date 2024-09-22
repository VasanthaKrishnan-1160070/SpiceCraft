namespace SpiceCraft.Server.DTO.Order;

public class UserOrderDTO
{
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public string OrderDate { get; set; }
    public string PaymentStatus { get; set; }
    public string OrderStatus { get; set; }
    public string ShippingInfo { get; set; }
    public decimal TotalCost { get; set; }
}