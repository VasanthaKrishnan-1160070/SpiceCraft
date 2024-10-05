using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Inventory;
using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private SpiceCraftContext _context;

        public OrderRepository(SpiceCraftContext context)
        {
            _context = context;
        }

        // Insert Order Items from Shopping Cart
        public async Task<int> InsertOrderItemsFromShoppingCartAsync(int userId, int orderId)
        {
            // Get cart item count
            var cartItemCount = await _context.CartItems
                .Where(ci => ci.Cart.UserId == userId && !ci.Cart.IsOrdered)
                .CountAsync();

            if (cartItemCount == 0)
            {
                return 0;
            }

            // Insert order details
            var cartItems = await (from ci in _context.CartItems
                join p in _context.Items on ci.ItemId equals p.ItemId
                join pp in _context.PromotionItems on p.ItemId equals pp.ItemId into ppGroup
                from pp in ppGroup.DefaultIfEmpty()
                join prc in _context.PromotionCategories on p.CategoryId equals prc.CategoryId into prcGroup
                from prc in prcGroup.DefaultIfEmpty()
                where ci.Cart.UserId == userId && !ci.Cart.IsOrdered
                select new OrderDetail
                {
                    OrderId = orderId,
                    ItemId = p.ItemId,
                    ActualPrice = p.Price,
                    DiscountRate = pp != null ? pp.DiscountRate : (prc != null ? prc.DiscountRate : 0),
                    Quantity = ci.Quantity,
                    PurchasePrice = p.Price * ci.Quantity *
                                    (1 - ((pp != null ? pp.DiscountRate : (prc != null ? prc.DiscountRate : 0)) / 100)),
                    Description = ci.Description
                }).ToListAsync();

            if (cartItems.Count == 0)
            {
                return 0;
            }

            _context.OrderDetails.AddRange(cartItems);
            await _context.SaveChangesAsync();

            // Adjust inventory for the products
            var updateInventoryQuery = from od in _context.OrderDetails
                join i in _context.Inventories on od.ItemId equals i.ItemId
                where od.OrderId == orderId
                select new { i, od.Quantity };

            foreach (var item in updateInventoryQuery)
            {
                item.i.CurrentStock -= item.Quantity;
            }

            await _context.SaveChangesAsync();

            // Remove items from the shopping cart
            var cartId = await _context.ShoppingCarts
                .Where(sc => sc.UserId == userId && !sc.IsOrdered)
                .Select(sc => sc.CartId)
                .FirstOrDefaultAsync();

            if (cartId != null)
            {
                var cartItemsToRemove = _context.CartItems.Where(ci => ci.CartId == cartId);
                _context.CartItems.RemoveRange(cartItemsToRemove);
                await _context.SaveChangesAsync();
            }

            return 1;
        }

        // Insert Order Items for Corporate Client
        public async Task<int> InsertOrderItemsForCorporateClientAsync(int userId, int orderId)
        {
            // Get cart item count
            var cartItemCount = await _context.CartItems
                .Where(ci => ci.Cart.UserId == userId && !ci.Cart.IsOrdered)
                .CountAsync();

            if (cartItemCount == 0)
            {
                return 0;
            }

            // Insert order details for corporate client
            var cartItems = await (from ci in _context.CartItems
                join p in _context.Items on ci.ItemId equals p.ItemId
                join pbp in _context.PromotionBulkItems on p.ItemId equals pbp.ItemId into pbpGroup
                from pbp in pbpGroup.DefaultIfEmpty()
                where ci.Cart.UserId == userId && !ci.Cart.IsOrdered
                select new OrderDetail
                {
                    OrderId = orderId,
                    ItemId = p.ItemId,
                    ActualPrice = p.Price,
                    DiscountRate = (pbp != null && ci.Quantity >= pbp.RequiredQuantity) ? pbp.DiscountRate : 0,
                    Quantity = ci.Quantity,
                    PurchasePrice = (pbp != null && ci.Quantity >= pbp.RequiredQuantity)
                        ? p.Price * ci.Quantity * (1 - pbp.DiscountRate / 100)
                        : p.Price * ci.Quantity,
                    Description = ci.Description
                }).ToListAsync();

            if (!cartItems.Any())
            {
                return 0;
            }

            _context.OrderDetails.AddRange(cartItems);
            await _context.SaveChangesAsync();

            // Adjust inventory for the products
            var updateInventoryQuery = from od in _context.OrderDetails
                join i in _context.Inventories on od.ItemId equals i.ItemId
                where od.OrderId == orderId
                select new { i, od.Quantity };

            foreach (var item in updateInventoryQuery)
            {
                item.i.CurrentStock -= item.Quantity;
            }

            await _context.SaveChangesAsync();

            // Remove items from the shopping cart
            var cartId = await _context.ShoppingCarts
                .Where(sc => sc.UserId == userId && !sc.IsOrdered)
                .Select(sc => sc.CartId)
                .FirstOrDefaultAsync();

            if (cartId != null)
            {
                var cartItemsToRemove = _context.CartItems.Where(ci => ci.CartId == cartId);
                _context.CartItems.RemoveRange(cartItemsToRemove);
                await _context.SaveChangesAsync();
            }

            return 1;
        }

        // Get Order Information
        public async Task<List<OrderDetailDTO>> GetOrderInfoAsync(int orderId)
        {
            var orderDetails = await (from od in _context.OrderDetails
                join p in _context.Items on od.ItemId equals p.ItemId
                where od.OrderId == orderId
                select new OrderDetailDTO
                {
                    OrderId = od.OrderId,
                    ItemId = p.ItemId,
                    Quantity = od.Quantity,
                    PurchasePrice = od.PurchasePrice,
                    ItemName = p.ItemName,
                    Price = p.Price
                }).ToListAsync();

            return orderDetails;
        }

        // Update Order Status
        public async Task<bool> UpdateOrderStatusAsync(int orderId, string orderStatus)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                return false;
            }

            order.OrderStatus = orderStatus;
            await _context.SaveChangesAsync();

            // Return true if the update was successful
            return order.OrderStatus == orderStatus;
        }

        // Get User Orders
        public async Task<List<UserOrderDTO>> GetUserOrdersAsync(int userId)
        {
            var userOrders = await (from o in _context.Orders
                join pyt in _context.Payments on o.OrderId equals pyt.OrderId into paymentGroup
                from pyt in paymentGroup.DefaultIfEmpty()
                // join shc in _context.CountryShippingOptions on o.CountryShippingOptionId equals shc
                //     .CountryShippingOptionId into shippingGroup
                // from shc in shippingGroup.DefaultIfEmpty()
                // join sh in _context.ShippingOptions on shc.ShippingOptionId equals sh.ShippingOptionId into
                //     shippingOptGroup
                // from sh in shippingOptGroup.DefaultIfEmpty()
                // join ct in _context.Countries on shc.CountryId equals ct.CountryId into countryGroup
                // from ct in countryGroup.DefaultIfEmpty()
                where o.UserId == userId
                orderby o.OrderDate descending
                select new UserOrderDTO
                {
                    UserId = o.UserId,
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                    PaymentStatus = pyt != null ? pyt.PaymentStatus : "Pending",
                    OrderStatus = o.OrderStatus,
                    // ShippingInfo = o.IsFreeShipping
                    //     ? "Free Shipping"
                    //     : (sh.ShippingOptionId != 4
                    //         ? $"{sh.ShippingOptionName} to {ct.Name}"
                    //         : sh.ShippingOptionName),
                    TotalCost = o.TotalCost
                }).ToListAsync();

            return userOrders;
        }

        public async Task<UserOrderDTO> GetFirstUnpaidUserOrdersAsync(int userId)
        {
            var userOrder = await (from o in _context.Orders
                join pyt in _context.Payments on o.OrderId equals pyt.OrderId into paymentGroup
                from pyt in paymentGroup.DefaultIfEmpty()
                // join shc in _context.CountryShippingOptions on o.CountryShippingOptionId equals shc
                //     .CountryShippingOptionId into shippingGroup
                // from shc in shippingGroup.DefaultIfEmpty()
                // join sh in _context.ShippingOptions on shc.ShippingOptionId equals sh.ShippingOptionId into
                //     shippingOptGroup
                // from sh in shippingOptGroup.DefaultIfEmpty()
                // join ct in _context.Countries on shc.CountryId equals ct.CountryId into countryGroup
                // from ct in countryGroup.DefaultIfEmpty()
                where o.UserId == userId && (o.OrderStatus == "Pending" || o.OrderStatus == "Prepared")
                orderby o.OrderDate descending
                select new UserOrderDTO
                {
                    UserId = o.UserId,
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                    PaymentStatus = pyt != null ? pyt.PaymentStatus : "Pending",
                    OrderStatus = o.OrderStatus,
                    // ShippingInfo = o.IsFreeShipping
                    //     ? "Free Shipping"
                    //     : (sh.ShippingOptionId != 4
                    //         ? $"{sh.ShippingOptionName} to {ct.Name}"
                    //         : sh.ShippingOptionName),
                    TotalCost = o.TotalCost
                }).FirstOrDefaultAsync();

            return userOrder;
        }
        
        // Get Inventory details for a specific product
        public async Task<ProductInventoryDTO> GetInventoryForProductAsync(int itemId)
        {
            var inventory = await (from i in _context.Inventories
                where i.ItemId == itemId
                select new ProductInventoryDTO
                {
                    ItemId = i.ItemId,
                    AvailableStock = i.CurrentStock,
                }).FirstOrDefaultAsync();

            if (inventory == null)
            {
                throw new Exception($"Inventory not found for product with ID: {itemId}");
            }

            return inventory;
        }

        public async Task<bool> CreateUserOrderAsync(OrderDTO order)
        {
            var newOrder = new Order
            {
                UserId = order.UserId,
                OrderDate = DateTime.Now,
                TotalCost = order.TotalCost,
                OrderStatus = order.OrderStatus,
                IsFreeShipping = order.IsFreeShipping,
                Preference = order.Preference,
                ShippingOptionId = order.ShippingOptionId
            };

            var entity = _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return newOrder.OrderId > 0;
        }
    }
}
