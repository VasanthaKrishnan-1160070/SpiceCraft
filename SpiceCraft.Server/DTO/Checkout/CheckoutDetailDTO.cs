using SpiceCraft.Server.DTO.User;

namespace SpiceCraft.Server.DTO.Checkout;

public class CheckoutDetailDTO
{
    public UserAddressDTO UserAddress { get; set; }
    public decimal SubTotal { get; set; }
    public decimal ToPay { get; set; }
    public decimal CreditLimit { get; set; }
    public decimal CreditUsed { get; set; }
    public decimal CreditBalance { get; set; }
    public string CountryName { get; set; }
    public List<ShippingOptionDTO> ShippingOptions { get; set; }
    public bool QualifiedForFreeShipping { get; set; }
}