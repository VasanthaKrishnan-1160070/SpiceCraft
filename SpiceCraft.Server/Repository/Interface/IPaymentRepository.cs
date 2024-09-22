using SpiceCraft.Server.DTO.Payment;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IPaymentRepository
    {
        // Retrieves payment details for a specific user by userId
        Task<List<PaymentDTO>> GetPaymentsForUserAsync(int userId);

        // Retrieves payment details for all internal users
        Task<List<PaymentDTO>> GetPaymentsForInternalUsersAsync();

        // Retrieves detailed invoice information for a specific payment transaction
        Task<PaymentInvoiceDTO> GetPaymentInvoiceDetailsAsync(int transactionId);

        Task<bool> CreateUserPaymentAsync(PaymentDTO paymentDto);
    }
}
