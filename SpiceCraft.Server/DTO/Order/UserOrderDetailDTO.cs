using SpiceCraft.Server.DTO.User;

namespace SpiceCraft.Server.DTO.Order;

public class UserOrderDetailDTO
{
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public string OrderDate { get; set; }
    public UserAddressDTO ShippingAddress { get; set; }
    public ContactInfoDTO ContactInfo { get; set; }
    public string OrderStatus { get; set; }
    public decimal TotalCost { get; set; }
    public IEnumerable<OrderDetailDTO> OrderItems { get; set; }
    
    public int ShippingOptionId { get; set; }
}