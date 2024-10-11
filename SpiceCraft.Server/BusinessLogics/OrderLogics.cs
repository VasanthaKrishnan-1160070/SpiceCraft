using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{
    public class OrderLogics : IOrderLogics
    {
        private readonly IOrderRepository _orderRepo;

        public OrderLogics(IOrderRepository orderService)
        {
            _orderRepo = orderService;
        }

        // Place an order from the shopping cart for a regular user
        public async Task<ResultDetail<bool>> PlaceOrderAsync(int userId, int orderId)
        {
            int result = await _orderRepo.InsertOrderItemsFromShoppingCartAsync(userId, orderId);
            if (result == 0)
            {
                return HelperFactory.Msg.Error<bool>("No items in the cart to place an order.");
            }

            bool isOrderStatusUpdated = await _orderRepo.UpdateOrderStatusAsync(orderId, "Placed");
            if (!isOrderStatusUpdated)
            {
                return HelperFactory.Msg.Error<bool>("Failed to update the order status.");
            }

            var orderDetails = await _orderRepo.GetOrderDetailsAsync(orderId);  // Update the order details in the database

            if (orderDetails != null)
            {
                var emailService = new EmailHelper();
                // Send email notification to the customer
                Task.Run(() => emailService.SendNewOrderConfirmationEmailAsync("", "", orderDetails));
            }
            
            return HelperFactory.Msg.Success(true);
        }

        // Place an order for a corporate client
        public async Task<ResultDetail<bool>> PlaceCorporateClientOrderAsync(int userId, int orderId)
        {
            int result = await _orderRepo.InsertOrderItemsForCorporateClientAsync(userId, orderId);
            if (result == 0)
            {
                return HelperFactory.Msg.Error<bool>("No items in the cart to place an order.");
            }

            bool isOrderStatusUpdated = await _orderRepo.UpdateOrderStatusAsync(orderId, "Placed");
            if (!isOrderStatusUpdated)
            {
                return HelperFactory.Msg.Error<bool>("Failed to update the order status.");
            }

            return HelperFactory.Msg.Success(true);
        }

        // Get detailed information about an order
        public async Task<ResultDetail<List<OrderDetailDTO>>> GetOrderDetailsAsync(int orderId)
        {
            var orderDetails = await _orderRepo.GetOrderInfoAsync(orderId);
            if (orderDetails == null || !orderDetails.Any())
            {
                return HelperFactory.Msg.Error<List<OrderDetailDTO>>("No details found for the order.");
            }

            return HelperFactory.Msg.Success(orderDetails);
        }

        public async Task<ResultDetail<UserOrderDetailDTO>> GetUserOrderDetailsAsync(int orderId)
        {
            var userOrderDetails =  await _orderRepo.GetOrderDetailsAsync(orderId);
            if (userOrderDetails == null || !userOrderDetails.OrderItems.Any())
            {
                return HelperFactory.Msg.Error<UserOrderDetailDTO>("No details found for the order.");
            }
            
            return HelperFactory.Msg.Success(userOrderDetails);
        }

        // Get all orders for a user
        public async Task<ResultDetail<List<UserOrderDTO>>> GetUserOrdersAsync(int userId)
        {
            var orders = await _orderRepo.GetUserOrdersAsync(userId);
            if (orders == null || !orders.Any())
            {
                return HelperFactory.Msg.Error<List<UserOrderDTO>>("No orders found for the user.");
            }

            return HelperFactory.Msg.Success(orders);
        }
        
        public async Task<ResultDetail<List<UserOrderDTO>>> GetOrdersAsync()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            if (orders == null || !orders.Any())
            {
                return HelperFactory.Msg.Error<List<UserOrderDTO>>("No orders found.");
            }

            return HelperFactory.Msg.Success(orders);
        }
        
        public async Task<ResultDetail<UserOrderDTO>> GetFirstUnpaidUserOrdersAsync(int userId)
        {
            var firstUnpaidOrder = await _orderRepo.GetFirstUnpaidUserOrdersAsync(userId);
            if (firstUnpaidOrder == null)
            {
                return HelperFactory.Msg.Error<UserOrderDTO>("No unpaid orders found for the user.");
            }

            return HelperFactory.Msg.Success(firstUnpaidOrder);
        }

        // Change the order status
        public async Task<ResultDetail<bool>> ChangeOrderStatusAsync(int orderId, string newStatus)
        {
            bool result = await _orderRepo.UpdateOrderStatusAsync(orderId, newStatus);
            if (!result)
            {
                return HelperFactory.Msg.Error<bool>("Failed to update order status.");
            }

            return HelperFactory.Msg.Success(true);
        }

        // Verify stock availability before placing an order (additional business rule)
        public async Task<ResultDetail<bool>> VerifyStockBeforeOrderAsync(int userId)
        {
            var userOrders = await _orderRepo.GetUserOrdersAsync(userId);
            if (userOrders == null || !userOrders.Any())
            {
                return HelperFactory.Msg.Error<bool>("No orders to verify stock for.");
            }

            foreach (var order in userOrders)
            {
                var orderDetails = await _orderRepo.GetOrderInfoAsync(order.OrderId);
                if (orderDetails != null)
                {
                    foreach (var item in orderDetails)
                    {
                        var inventory =
                            await _orderRepo
                                .GetInventoryForProductAsync(item
                                    .ItemId); // Assuming a GetInventoryForProduct method in service
                        if (inventory.AvailableStock < item.Quantity)
                        {
                            return HelperFactory.Msg.Error<bool>($"Insufficient stock for product: {item.ItemName}");
                        }
                    }
                }
            }

            return HelperFactory.Msg.Success(true);
        }

        public async Task<ResultDetail<bool>> CreateUserOrderAsync(OrderDTO order)
        {
            var status = await _orderRepo.CreateUserOrderAsync(order);
            return status? HelperFactory.Msg.Success(true) : HelperFactory.Msg.Error<bool>("Failed to create order.");
        }

        public async Task<ResultDetail<bool>> InsertOrderItemsFromShoppingCartAsync(int userId, int orderId)
        {
            var status = await _orderRepo.InsertOrderItemsFromShoppingCartAsync(userId, orderId);
            return status > 0 ? HelperFactory.Msg.Success(true) : HelperFactory.Msg.Error<bool>("Failed to insert order items from shopping cart.");
        }
        
        // Business Logic to update order
        public async Task<ResultDetail<bool>> UpdateOrderAsync(int orderId, UserOrderDetailDTO updatedOrderDetails)
        {
            // Validate input
            if (updatedOrderDetails == null || orderId <= 0)
            {
                return new ResultDetail<bool>
                {
                    IsSuccess = false,
                    Message = "Invalid order details provided."
                };
            }

            // Check if order exists before updating
            var existingOrder = await _orderRepo.GetOrderDetailsAsync(orderId);
            if (existingOrder == null)
            {
                return new ResultDetail<bool>
                {
                    IsSuccess = false,
                    Message = "Order not found."
                };
            }

            // Perform any necessary business logic before updating
            if (updatedOrderDetails.TotalCost <= 0)
            {
                return new ResultDetail<bool>
                {
                    IsSuccess = false,
                    Message = "Total cost must be greater than zero."
                };
            }

            // Call repository to update the order
            var updateSuccess = await _orderRepo.UpdateOrderAsync(orderId, updatedOrderDetails);

            if (!updateSuccess)
            {
                return new ResultDetail<bool>
                {
                    IsSuccess = false,
                    Message = "Failed to update the order."
                };
            }
            
            if (updatedOrderDetails != null)
            {
                var emailService = new EmailHelper();
                // Send email notification to the customer
                Task.Run(() => emailService.SendOrderStatusChangeEmailAsync("", "",
                    updatedOrderDetails.OrderId.ToString(), updatedOrderDetails.OrderStatus));
            }

            return new ResultDetail<bool>
            {
                IsSuccess = true,
                Message = "Order updated successfully."
            };
        }
    }
}
