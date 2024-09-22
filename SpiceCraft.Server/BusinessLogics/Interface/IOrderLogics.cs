using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IOrderLogics
    {
        // Place an order from the shopping cart for a regular user
        Task<ResultDetail<bool>> PlaceOrderAsync(int userId, int orderId);

        // Place an order for a corporate client
        Task<ResultDetail<bool>> PlaceCorporateClientOrderAsync(int userId, int orderId);

        // Get detailed information about an order
        Task<ResultDetail<List<OrderDetailDTO>>> GetOrderDetailsAsync(int orderId);

        // Get all orders for a user
        Task<ResultDetail<List<UserOrderDTO>>> GetUserOrdersAsync(int userId);

        // Change the order status
        Task<ResultDetail<bool>> ChangeOrderStatusAsync(int orderId, string newStatus);

        // Verify stock availability before placing an order
        Task<ResultDetail<bool>> VerifyStockBeforeOrderAsync(int userId);
    }
}
