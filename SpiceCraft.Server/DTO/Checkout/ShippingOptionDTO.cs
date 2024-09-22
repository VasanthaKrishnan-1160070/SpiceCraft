namespace SpiceCraft.Server.DTO.Checkout;

public class ShippingOptionDTO
{
    public int ShippingOptionId { get; set; }
    public string ShippingOptionName { get; set; }
    public decimal FreeShippingThreshold { get; set; }
    public decimal Cost { get; set; }
    public string CountryName { get; set; }
}