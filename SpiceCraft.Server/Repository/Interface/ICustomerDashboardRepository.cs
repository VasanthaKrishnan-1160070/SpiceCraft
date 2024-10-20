namespace SpiceCraft.Server.Repository.Interface;

public interface ICustomerDashboardRepository
{
    Task<int> GetTotalOrdersAsync(int userId);
    Task<int> GetShippedOrdersAsync(int userId);
    Task<int> GetCancelledOrdersAsync(int userId);
    Task<decimal> GetGiftCardBalanceAsync(int userId);
    Task<int> GetCartItemsCountAsync(int userId);
    Task<int> GetNotificationCountAsync(int userId);
}