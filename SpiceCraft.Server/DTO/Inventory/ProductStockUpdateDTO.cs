namespace SpiceCraft.Server.DTO.Inventory;

public class ProductStockUpdateDTO
{
    public int CurrentStock { get; set; }
    public int LowStockThreshold { get; set; }
}