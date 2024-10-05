namespace SpiceCraft.Server.DTO.Payment;

// PaymentDTO for basic payment information
public class PaymentDTO
{
    public int PaymentId { get; set; }
    public int UserId { get; set; }
    public int TransactionId { get; set; }
    public decimal PaymentAmount { get; set; }
    public int OrderId { get; set; }
    public string PaymentStatus { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = "credit card"; // Default payment method
}