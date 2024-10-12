namespace SpiceCraft.Server.DTO.Report;

public class OrderReportDTO
{
    public int Year { get; set; }
    public int Month { get; set; }
    
    public string MonthName { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalSales { get; set; }
}