namespace SpiceCraft.Server.DTO.Payment;

// PaymentDTO for basic payment information
public class PaymentDTO
{
    public int TransactionId { get; set; }
    public decimal PaymentAmount { get; set; }
    public int OrderId { get; set; }
    public string PaymentStatus { get; set; }
    public string PaymentDate { get; set; }
}