namespace SpiceCraft.Server.DTO.Cart;

public class ShoppingCartDTO : IResponse
{
    public List<CartItemDTO> CartItems { get; set; }
    public decimal TotalPrice { get; set; }
}