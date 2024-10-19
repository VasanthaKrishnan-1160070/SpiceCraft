using SpiceCraft.Server.DTO.Customer;

namespace SpiceCraft.Server.BusinessLogics.Interface;

public interface ICustomerDashboardLogics
{
    Task<CustomerDashboardDTO> GetCustomerDashboardDataAsync(int userId);
}