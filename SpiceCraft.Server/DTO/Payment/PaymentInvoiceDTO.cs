using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.DTO.User;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.DTO.Payment;

// PaymentInvoiceDTO for detailed invoice information
public class PaymentInvoiceDTO
{
    public int UserId { get; set; }
    public int OrderId { get; set; }
    public decimal Amount { get; set; }
    public string PaymentDate { get; set; }
    public string PaymentMethod { get; set; }
    public string PaymentStatus { get; set; }
    public string ShippingOptionName { get; set; }
    public string CountryName { get; set; }
    public bool IsFreeShipping { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal SubTotal { get; set; }
    public bool IsPickUp { get; set; }

    public List<OrderDetailDTO> OrderDetails { get; set; }
    public UserAddress UserAddress { get; set; }
    public ContactInfoDTO ContactInfo { get; set; }
}