using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Cart;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{
    public class CartLogics : ICartLogics
    {
        private readonly ICartRepository _shoppingCartService;

    public CartLogics(ICartRepository shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }

    public async Task<ResultDetail<ShoppingCartDTO>> GetShoppingCartByUserIdAsync(int userId)
    {
        var shoppingCart = await _shoppingCartService.GetShoppingCartsByUserIdAsync(userId);
        if (shoppingCart != null && shoppingCart.CartItems.Any())
        {
            return HelperFactory.Msg.Success(shoppingCart, "Cart retrieved successfully.");
        }
        return HelperFactory.Msg.Error<ShoppingCartDTO>("No items in the cart.");
    }

    public async Task<ResultDetail<ShoppingCartDTO>> GetShoppingCartForCorporateClientsAsync(int userId)
    {
        var shoppingCart = await _shoppingCartService.GetShoppingCartsByUserIdForCorporateClientsAsync(userId);
        if (shoppingCart != null && shoppingCart.CartItems.Any())
        {
            return HelperFactory.Msg.Success(shoppingCart, "Corporate client cart retrieved successfully.");
        }
        return HelperFactory.Msg.Error<ShoppingCartDTO>("No items in the corporate client cart.");
    }

    public async Task<ResultDetail<decimal>> GetTotalCartPriceAsync(int userId)
    {
        var totalPrice = await _shoppingCartService.GetTotalCartPrice(userId);
        return totalPrice > 0 
            ? HelperFactory.Msg.Success(totalPrice, "Total price calculated successfully.") 
            : HelperFactory.Msg.Error<decimal>("Failed to calculate the total price.");
    }

    public async Task<ResultDetail<string>> IncrementCartItemQuantityAsync(int cartItemId, int quantity)
    {
        await _shoppingCartService.IncrementCartItemQtyAsync(cartItemId, quantity);
        return HelperFactory.Msg.Success("Quantity increased successfully.");
    }

    public async Task<ResultDetail<string>> DecrementCartItemQuantityAsync(int cartItemId, int quantity)
    {
        await _shoppingCartService.DecrementCartItemQtyAsync(cartItemId, quantity);
        return HelperFactory.Msg.Success("Quantity decreased successfully.");
    }

    public async Task<ResultDetail<string>> AddOrUpdateCartItemAsync(CartItemDTO cartItemDTO)
    {
        await _shoppingCartService.CreateOrUpdateCartItemAsync(cartItemDTO);
        return HelperFactory.Msg.Success("Cart item added/updated successfully.");
    }

    public async Task<ResultDetail<string>> RemoveCartItemAsync(int cartItemId)
    {
        await _shoppingCartService.DeleteCartItemAsync(cartItemId);
        return HelperFactory.Msg.Success("Cart item removed successfully.");
    }
    }
}
