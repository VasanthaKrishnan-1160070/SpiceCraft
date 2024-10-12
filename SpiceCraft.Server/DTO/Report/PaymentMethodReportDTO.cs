namespace SpiceCraft.Server.DTO.Report;

public class PaymentMethodReportDTO
{
    public string PaymentMethod { get; set; }
    public int TotalPayments { get; set; }
    public decimal TotalAmount { get; set; }
}