namespace SpiceCraft.Server.DTO.Report;

public class MostSoldProductsDTO
{
    public int ItemId { get; set; }
    public int TotalQuantitySold { get; set; }
    public decimal TotalSales { get; set; }
    
    public string ItemName { get; set; }
}