using SpiceCraft.Server.DTO.Cart;
using SpiceCraft.Server.Helpers.Request;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface ICartRepository
    {
        Task<int> CreateOrGetShoppingCartAsync(int userId);
        Task<ShoppingCartDTO> GetShoppingCartsByUserIdAsync(int userId);
        Task<ShoppingCartDTO> GetShoppingCartsByUserIdForCorporateClientsAsync(int userId);
        Task<decimal> GetTotalCartPrice(int userId);
        Task<decimal> GetTotalCartPriceForCorporateClients(int userId);
        Task<CartItemDTO> GetCartItemByIdAsync(int cartItemId);
        Task<CartItemDTO> GetCartItemAsync(int cartId, int productId);
        Task IncrementCartItemQtyAsync(int cartItemId, int quantity);
        Task DecrementCartItemQtyAsync(int cartItemId, int quantity);
        Task CreateOrUpdateCartItemAsync(CreateUpdateCartItemRequest cartItem);
        Task DeleteCartItemAsync(int cartItemId);
    }
}
