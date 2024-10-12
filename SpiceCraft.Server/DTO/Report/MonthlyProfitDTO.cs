namespace SpiceCraft.Server.DTO.Report;

public class MonthlyProfitDTO
{
    public int Year { get; set; }
    public int Month { get; set; }
    
    public string MonthName { get; set; }
    public decimal TotalSales { get; set; }
    public decimal TotalCost { get; set; }
    public decimal Profit { get; set; }
}