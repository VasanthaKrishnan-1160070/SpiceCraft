using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Checkout;
using SpiceCraft.Server.DTO.GiftCard;
using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.DTO.Payment;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{
    public class CheckoutLogics : ICheckoutLogics
    {
        private readonly IOrderLogics _orderBusinessLogic;
        private readonly IPaymentLogics _paymentBusinessLogic;
        private readonly ICartRepository _shoppingCartService;
        private readonly IUserRepository _userService; // Assuming a user service to get session info and user data
        private readonly IShippingRepository _shippingService;
        private readonly IGiftCardRepository _giftCardService;
        private readonly IUserRepository _corporateClientService;

        private decimal _freeShippingThreshold = 15; // need to be fetched from db setting table

        public CheckoutLogics(
            IOrderLogics orderBusinessLogic,
            IPaymentLogics paymentBusinessLogic,
            ICartRepository shoppingCartService,
            IUserRepository userService,
            IShippingRepository shippingService,
            IGiftCardRepository giftCardService,
            IUserRepository corporateClientService)
        {
            _orderBusinessLogic = orderBusinessLogic;
            _paymentBusinessLogic = paymentBusinessLogic;
            _shoppingCartService = shoppingCartService;
            _userService = userService;
            _shippingService = shippingService;
            _giftCardService = giftCardService;
            _corporateClientService = corporateClientService;
        }

        // Property to check if the user is a customer
        private bool IsCustomer => true; // _userService.IsUserCustomer();

        // Property to check if the user is a corporate client
        private bool IsCorporateClient => false; // _userService.IsUserCorporateClient();

        // Checkout info for normal customer or corporate clients
        public async Task<ResultDetail<CheckoutDetailDTO>> GetCheckoutInfoAsync(int userId)
        {
            if (IsCustomer)
            {
                return await GetCustomerCheckoutInfoAsync(userId);
            }
            else if (IsCorporateClient)
            {
                return await GetCorporateClientCheckoutInfoAsync(userId);
            }

            return HelperFactory.Msg.Error<CheckoutDetailDTO>("Could not fetch checkout details");
        }

        // Checkout info for corporate client
        private async Task<ResultDetail<CheckoutDetailDTO>> GetCorporateClientCheckoutInfoAsync(int userId)
        {
            var checkoutDetail = new CheckoutDetailDTO();
            var finalPrice = await _shoppingCartService.GetTotalCartPriceForCorporateClients(userId);
            var userAddress = await _userService.GetUserAddressByIdAsync(userId);
           // var shippingOptions = await _shippingService.GetShippingOptionsAsync(userAddress.CountryId);
            // var creditInfo = await _corporateClientService.GetCreditLimitAsync(userId);

            if (finalPrice != null && userAddress != null)
            {
                decimal subTotal = finalPrice;
                decimal freeShippingThreshold = _freeShippingThreshold;
                bool qualifiedForFreeShipping = subTotal > freeShippingThreshold;

                checkoutDetail = new CheckoutDetailDTO
                {
                    UserAddress = userAddress,
                    SubTotal = subTotal,
                    ToPay = subTotal,
                  //  CreditLimit = creditInfo.CreditLimit,
                  //  CreditUsed = creditInfo.CreditUsed,
                  //  CreditBalance = Math.Max(creditInfo.CreditLimit - creditInfo.CreditUsed, 0),
                  //  CountryName = shippingOptions.First().CountryName,
                  //  ShippingOptions = shippingOptions,
                    QualifiedForFreeShipping = qualifiedForFreeShipping
                };
                return HelperFactory.Msg.Success(checkoutDetail);
            }

            return HelperFactory.Msg.Error<CheckoutDetailDTO>("Could not fetch checkout details");
        }

        // Gets customer checkout info
        private async Task<ResultDetail<CheckoutDetailDTO>> GetCustomerCheckoutInfoAsync(int userId)
        {
            var checkoutDetail = new CheckoutDetailDTO();
            var finalPrice = await _shoppingCartService.GetTotalCartPrice(userId);
            var userAddress = await _userService.GetUserAddressByIdAsync(userId);
            var shippingOptions = await _shippingService.GetShippingOptionsAsync();

            if (finalPrice != null && userAddress != null)
            {
                decimal subTotal = finalPrice;
                decimal freeShippingThreshold = _freeShippingThreshold;
                bool qualifiedForFreeShipping = subTotal > freeShippingThreshold;

                checkoutDetail = new CheckoutDetailDTO
                {
                    UserAddress = userAddress,
                    SubTotal = subTotal,
                    ToPay = subTotal,
                    ShippingOptions = shippingOptions.ToList(),
                    QualifiedForFreeShipping = qualifiedForFreeShipping
                };
                return HelperFactory.Msg.Success(checkoutDetail);
            }

            return HelperFactory.Msg.Error<CheckoutDetailDTO>("Could not fetch checkout details");
        }

        // Calculate GST amount
        public decimal GetGSTAmount(decimal totalAmount)
        {
            return totalAmount * 0.15M;
        }

        // Get total amount to pay including shipping and GST
        public async Task<ResultDetail<decimal>> GetTotalAmountToPayAsync(int userId, int? shippingOptionId)
        {
            var checkoutDetailResult = await GetCheckoutInfoAsync(userId);
            if (!checkoutDetailResult.IsSuccess)
            {
                return HelperFactory.Msg.Error<decimal>("Could not fetch checkout details");
            }

            var checkoutDetail = checkoutDetailResult.Data;
            decimal totalToPay = checkoutDetail.SubTotal;

            if (shippingOptionId.HasValue)
            {
                // var shippingInfoResult = await GetCountryShippingInfoAsync(shippingOptionId.Value);
                // if (!shippingInfoResult.IsSuccess)
                // {
                //     return shippingInfoResult;
                // }

                // var shippingInfo = shippingInfoResult.Data;
                if (checkoutDetail.SubTotal > _freeShippingThreshold && !IsCorporateClient)
                {
                    totalToPay = checkoutDetail.SubTotal;
                }
                else
                {
                    totalToPay += 5; // shippingInfo.Cost;
                }
            }

            totalToPay += GetGSTAmount(totalToPay);
            return HelperFactory.Msg.Success(totalToPay);
        }

        // Apply gift card to the final amount
        public async Task<ResultDetail<GiftCardDTO>> ApplyGiftCardAsync(string code, int userId)
        {
            var giftCard = _giftCardService.GetGiftCardByCode(code);
            if (giftCard == null)
            {
                return HelperFactory.Msg.Error<GiftCardDTO>("Either the code is invalid or the Gift card not found");
            }

            if (giftCard.Balance <= 0)
            {
                return HelperFactory.Msg.Error<GiftCardDTO>("Gift card balance is insufficient");
            }

            return HelperFactory.Msg.Success(giftCard,
                $"Gift card applied successfully, available balance is ${giftCard.Balance}");
        }

        // Creates user payment and inserts into the payment table
        public async Task<ResultDetail<PaymentDTO>> CreateUserPaymentAsync(UserOrderDTO order, string paymentMethod)
        {
            var paymentStatus = "Paid";
            if (paymentMethod == "client credit" && IsCorporateClient)
            {
                paymentStatus = "Not Paid";
            }

            var payment = new PaymentDTO
            {
                UserId = order.UserId,
                OrderId = order.OrderId,
                PaymentDate = DateTime.Now,
                PaymentStatus = paymentStatus,
                PaymentAmount = order.TotalCost
            };

            var paymentResult = await _paymentBusinessLogic.CreateUserPaymentAsync(payment);
            if (!paymentResult.IsSuccess)
            {
                return HelperFactory.Msg.Error<PaymentDTO>("Could not create payment");
            }

            return HelperFactory.Msg.Success(payment, "Created user payment successfully");
        }

        // Creates user order and returns the order
        public async Task<ResultDetail<bool>> CreateUserOrderAsync(int userId, int? shippingOptionId)
        {
            var totalAmountResult = await GetTotalAmountToPayAsync(userId, shippingOptionId);
            // if (!totalAmountResult.IsSuccess)
            // {
            //     return totalAmountResult;
            // }

            var order = new OrderDTO
            {
                UserId = userId,
                TotalCost = totalAmountResult.Data,
                OrderDate = DateTime.Now,
                IsFreeShipping = !shippingOptionId.HasValue,
                CountryShippingOptionId = shippingOptionId,
                OrderStatus = "Prepared"
            };

            var orderResult = await _orderBusinessLogic.CreateUserOrderAsync(order);
            // if (!orderResult.IsSuccess)
            // {
            //     return HelperFactory.Msg.Error<OrderDTO>("Could not create order");
            // }

            return orderResult;
        }

        // Update gift card balance after application
        // public async Task<ResultDetail> UpdateGiftCardBalanceAsync(string code, decimal orderTotal)
        // {
        //    // var giftCard = await _giftCardService.GetGiftCardByCodeAsync(code);
        //     if (giftCard == null)
        //     {
        //         return HelperFactory.Msg.Error("Gift card not found");
        //     }
        //
        //     decimal newBalance = Math.Max(giftCard.Balance - orderTotal, 0);
        //     var updateResult = await _giftCardService.UpdateGiftCardBalanceAsync(code, newBalance);
        //
        //     return updateResult
        //         ? HelperFactory.Msg.Success("Gift card balance updated successfully")
        //         : HelperFactory.Msg.Error("Could not update gift card balance");
        // }

        // Final checkout process including payment and order creation
        public async Task<ResultDetail<bool>> StartPostPaymentProcessAsync(int userId, int? shippingOptionId,
            string paymentMethod = "credit card", string giftCardCode = null)
        {
            var orderResult = await CreateUserOrderAsync(userId, shippingOptionId);
            // if (!orderResult.IsSuccess)
            // {
            //     return orderResult;
            // }
            var userOrder = await _orderBusinessLogic.GetFirstUnpaidUserOrdersAsync(userId);

            var paymentResult = await CreateUserPaymentAsync(userOrder.Data, paymentMethod);
            // if (!paymentResult.IsSuccess)
            // {
            //     return paymentResult;
            // }

            
            var orderItemsResult = await _orderBusinessLogic.InsertOrderItemsFromShoppingCartAsync(userId, userOrder?.Data.OrderId ?? 0);
            // if (!orderItemsResult.IsSuccess)
            // {
            //     return orderItemsResult;
            // }

            // if (!string.IsNullOrEmpty(giftCardCode))
            // {
            //     var updateGiftCardBalanceResult =
            //         await UpdateGiftCardBalanceAsync(giftCardCode, orderResult.Data.TotalCost);
            //     if (!updateGiftCardBalanceResult.IsSuccess)
            //     {
            //         return updateGiftCardBalanceResult;
            //     }
            // }

            // if (IsCorporateClient && paymentMethod != "credit card")
            // {
            //     await UpdateCreditLimitAsync(userId, orderResult.Data.TotalCost);
            // }

            if (orderItemsResult.IsSuccess && userOrder?.Data?.OrderId > 0)
            {
                await _orderBusinessLogic.ChangeOrderStatusAsync(userOrder.Data.OrderId, "Ready To Ship");
                orderItemsResult.Message = "Order placed successfully";
                
                // Get order details for email notification
                var orderDetails = await _orderBusinessLogic.GetUserOrderDetailsAsync(userOrder.Data.OrderId);
                if (orderDetails != null)
                {
                    var emailService = new EmailHelper();
                    // Send email notification to the customer
                    Task.Run(() => emailService.SendNewOrderConfirmationEmailAsync("", "", orderDetails.Data));
                }
            }
            else
            {
                orderItemsResult.Message = "Couldn't place the order please contact customer support";
            }

            return orderItemsResult;
        }

        // Update corporate client credit limit
        // public async Task<ResultDetail> UpdateCreditLimitAsync(int userId, decimal creditUsed)
        // {
        //     var result = await _corporateClientService.UpdateCreditLimitAsync(userId, creditUsed);
        //     return result
        //         ? HelperFactory.Msg.Success("Updated credit successfully")
        //         : HelperFactory.Msg.Error("Could not update credit");
        // }
    }
}
