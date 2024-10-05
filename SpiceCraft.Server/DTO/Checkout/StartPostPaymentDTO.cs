namespace SpiceCraft.Server.DTO.Checkout;

public class StartPostPaymentDTO
{
    public int UserId { get; set; }
    public int? ShippingOptionId { get; set; }
    public string PaymentMethod { get; set; }
    public string GiftCardCode { get; set; }
}