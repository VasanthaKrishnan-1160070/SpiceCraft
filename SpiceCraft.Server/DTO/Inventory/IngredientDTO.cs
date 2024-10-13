namespace SpiceCraft.Server.DTO.Inventory;

public class IngredientDTO
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; }
    public int CurrentStock { get; set; }
    public int ReorderLevel { get; set; }
}