using SpiceCraft.Server.DTO.Checkout;
using SpiceCraft.Server.DTO.GiftCard;
using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.DTO.Payment;
using SpiceCraft.Server.Helpers;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface ICheckoutLogics
    {
        // Fetches checkout information for a given user
        Task<ResultDetail<CheckoutDetailDTO>> GetCheckoutInfoAsync(int userId);

        // Calculates the GST amount for a given total amount
        decimal GetGSTAmount(decimal totalAmount);

        // Calculates the total amount to pay, including shipping and GST
        Task<ResultDetail<decimal>> GetTotalAmountToPayAsync(int userId, int? shippingOptionId);

        // Applies a gift card by code for a given user
        Task<ResultDetail<GiftCardDTO>> ApplyGiftCardAsync(string code, int userId);

        // Creates a payment for the user
        Task<ResultDetail<PaymentDTO>> CreateUserPaymentAsync(UserOrderDTO order, string paymentMethod);

        // Creates an order for the user
        Task<ResultDetail<bool>> CreateUserOrderAsync(int userId, int? shippingOptionId);

        // Starts the post-payment process, including order creation, payment, and gift card application
        Task<ResultDetail<bool>> StartPostPaymentProcessAsync(int userId, int? shippingOptionId, string paymentMethod = "credit card", string giftCardCode = null);
    }
}
