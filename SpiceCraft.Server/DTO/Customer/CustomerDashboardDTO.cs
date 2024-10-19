namespace SpiceCraft.Server.DTO.Customer;

public class CustomerDashboardDTO
{
    public int TotalOrders { get; set; }
    public int ShippedOrders { get; set; }
    public int CancelledOrders { get; set; }
    public decimal GiftCardBalance { get; set; }
    public int CartItemsCount { get; set; }
    public int UnreadNotifications { get; set; }
}