namespace SpiceCraft.Server.DTO.Order;

public class OrderDTO
{
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public decimal TotalCost { get; set; }
    public DateTime OrderDate { get; set; }
    public bool IsFreeShipping { get; set; }
    public int? CountryShippingOptionId { get; set; }
    public string OrderStatus { get; set; }
    
    public string Preference { get; set; } = string.Empty;

    public int ShippingOptionId { get; set; } = 1;
}