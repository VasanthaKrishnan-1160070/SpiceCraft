using SpiceCraft.Server.DTO.Cart;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Helpers.Request;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface ICartLogics
    {
        Task<ResultDetail<ShoppingCartDTO>> GetShoppingCartByUserIdAsync(int userId);
        Task<ResultDetail<ShoppingCartDTO>> GetShoppingCartForCorporateClientsAsync(int userId);
        Task<ResultDetail<decimal>> GetTotalCartPriceAsync(int userId);
        Task<ResultDetail<string>> IncrementCartItemQuantityAsync(int cartItemId, int quantity);
        Task<ResultDetail<string>> DecrementCartItemQuantityAsync(int cartItemId, int quantity);
        Task<ResultDetail<string>> AddOrUpdateCartItemAsync(CreateUpdateCartItemRequest cartItemRequest);
        Task<ResultDetail<string>> RemoveCartItemAsync(int cartItemId);
    }
}
