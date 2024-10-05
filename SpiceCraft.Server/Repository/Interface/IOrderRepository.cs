using SpiceCraft.Server.DTO.Inventory;
using SpiceCraft.Server.DTO.Order;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<int> InsertOrderItemsFromShoppingCartAsync(int userId, int orderId);
        Task<int> InsertOrderItemsForCorporateClientAsync(int userId, int orderId);
        Task<List<OrderDetailDTO>> GetOrderInfoAsync(int orderId);
        Task<UserOrderDTO> GetFirstUnpaidUserOrdersAsync(int userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string orderStatus);
        Task<List<UserOrderDTO>> GetUserOrdersAsync(int userId);
        Task<ProductInventoryDTO> GetInventoryForProductAsync(int productId);
        Task<UserOrderDetailDTO> GetOrderDetailsAsync(int orderId);
        Task<bool> CreateUserOrderAsync(OrderDTO order);
    }
}
