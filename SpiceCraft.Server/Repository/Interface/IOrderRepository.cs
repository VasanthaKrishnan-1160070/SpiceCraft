using SpiceCraft.Server.DTO.Inventory;
using SpiceCraft.Server.DTO.Order;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IOrderRepository
    {
        Task<int> InsertOrderItemsFromShoppingCartAsync(int userId, int orderId);
        Task<int> InsertOrderItemsForCorporateClientAsync(int userId, int orderId);
        Task<List<OrderDetailDTO>> GetOrderInfoAsync(int orderId);
        Task<List<UserOrderDTO>> GetAllOrdersAsync();
        Task<UserOrderDTO> GetFirstUnpaidUserOrdersAsync(int userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string orderStatus);
        Task<List<UserOrderDTO>> GetUserOrdersAsync(int userId);
        Task<ProductInventoryDTO> GetInventoryForProductAsync(int productId);
        Task<bool> CreateUserOrderAsync(OrderDTO order);
        Task<UserOrderDetailDTO> GetOrderDetailsAsync(int orderId);
        Task<bool> UpdateOrderAsync(int orderId, UserOrderDetailDTO updatedOrderDetails);
        Task<int> GetTodaysOrdersCountAsync();
        Task<int> GetOrdersToShipCountAsync();
        Task<decimal> GetTotalSalesTodayAsync();
        Task<decimal> GetTotalSalesThisMonthAsync();
    }
}
