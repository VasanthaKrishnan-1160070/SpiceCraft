namespace SpiceCraft.Server.DTO.Report;

public class ProductSalesByMonthDTO
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int ItemId { get; set; }
    
    public string MonthName { get; set; }
    public string ItemName { get; set; }
    public int TotalQuantitySold { get; set; }
    public decimal TotalSales { get; set; }
}