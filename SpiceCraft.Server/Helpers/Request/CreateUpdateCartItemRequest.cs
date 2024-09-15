namespace SpiceCraft.Server.Helpers.Request;

public class CreateUpdateCartItemRequest
{
    public int Quantity { get; set; }
    public int UserId { get; set; }
    public int CartId { get; set; } = 0;
    public int ItemId { get; set; }
    public decimal? PriceAtAdd { get; set; }
    public string Description { get; set; } = string.Empty;
}