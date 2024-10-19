using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Customer;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics;

public class CustomerDashboardLogics : ICustomerDashboardLogics
{
    private readonly ICustomerDashboardRepository _dashboardRepository;

    public CustomerDashboardLogics(ICustomerDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<CustomerDashboardDTO> GetCustomerDashboardDataAsync(int userId)
    {
        // Fetch data from the repository and populate the DTO
        var dashboardData = new CustomerDashboardDTO
        {
            TotalOrders = await _dashboardRepository.GetTotalOrdersAsync(userId),
            ShippedOrders = await _dashboardRepository.GetShippedOrdersAsync(userId),
            CancelledOrders = await _dashboardRepository.GetCancelledOrdersAsync(userId),
            GiftCardBalance = await _dashboardRepository.GetGiftCardBalanceAsync(userId),
            CartItemsCount = await _dashboardRepository.GetCartItemsCountAsync(userId),
            UnreadNotifications = await _dashboardRepository.GetNotificationCountAsync(userId)
        };

        return dashboardData;
    }
}
