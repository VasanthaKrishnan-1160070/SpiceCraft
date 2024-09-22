using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Payment;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics;

public class PaymentLogics : IPaymentLogics
{
    private readonly IPaymentRepository _paymentRepo;

    public PaymentLogics(IPaymentRepository paymentService)
    {
        _paymentRepo = paymentService;
    }

    // Get payments for a specific user
    public async Task<ResultDetail<List<PaymentDTO>>> GetPaymentsForUserAsync(int userId)
    {
        var payments = await _paymentRepo.GetPaymentsForUserAsync(userId);
        if (payments == null || payments.Count == 0)
        {
            return HelperFactory.Msg.Error<List<PaymentDTO>>("No payments found for the user.");
        }
        return HelperFactory.Msg.Success(payments);
    }

    // Get payments for all internal users
    public async Task<ResultDetail<List<PaymentDTO>>> GetPaymentsForInternalUsersAsync()
    {
        var payments = await _paymentRepo.GetPaymentsForInternalUsersAsync();
        if (payments == null || payments.Count == 0)
        {
            return HelperFactory.Msg.Error<List<PaymentDTO>>("No payments found for internal users.");
        }
        return HelperFactory.Msg.Success(payments);
    }

    // Get detailed invoice for a specific transaction
    public async Task<ResultDetail<PaymentInvoiceDTO>> GetPaymentInvoiceDetailsAsync(int transactionId)
    {
        var invoiceDetails = await _paymentRepo.GetPaymentInvoiceDetailsAsync(transactionId);
        if (invoiceDetails == null)
        {
            return HelperFactory.Msg.Error<PaymentInvoiceDTO>($"Invoice details not found for transaction ID: {transactionId}");
        }
        return HelperFactory.Msg.Success(invoiceDetails);
    }

    public async Task<ResultDetail<bool>> CreateUserPaymentAsync(PaymentDTO payment)
    {
        var status = await _paymentRepo.CreateUserPaymentAsync(payment);
        if (!status)
        {
            return HelperFactory.Msg.Error<bool>("Failed to create user payment.");
        }

        return HelperFactory.Msg.Success(status);
    }
}
