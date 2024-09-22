using SpiceCraft.Server.DTO.Payment;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IPaymentLogics
    {
        // Get payments for a specific user
        Task<ResultDetail<List<PaymentDTO>>> GetPaymentsForUserAsync(int userId);

        // Get payments for all internal users
        Task<ResultDetail<List<PaymentDTO>>> GetPaymentsForInternalUsersAsync();

        // Get detailed invoice for a specific transaction
        Task<ResultDetail<PaymentInvoiceDTO>> GetPaymentInvoiceDetailsAsync(int transactionId);

        Task<ResultDetail<bool>> CreateUserPaymentAsync(PaymentDTO payment);
    }
}
