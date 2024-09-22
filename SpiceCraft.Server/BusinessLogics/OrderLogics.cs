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
    }
}
