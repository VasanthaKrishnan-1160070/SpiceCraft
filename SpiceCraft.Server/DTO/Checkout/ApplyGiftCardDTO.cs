namespace SpiceCraft.Server.DTO.Checkout;

// Helper DTO classes for the controller request bodies
public class ApplyGiftCardDTO
{
    public string Code { get; set; }
    public int UserId { get; set; }
}