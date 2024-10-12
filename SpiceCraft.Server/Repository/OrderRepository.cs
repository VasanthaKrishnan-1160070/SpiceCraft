using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Inventory;
using SpiceCraft.Server.DTO.Order;
using SpiceCraft.Server.DTO.User;
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

            // Adjust inventory for the ingredients used in the products
            var updateInventoryQuery = from od in _context.OrderDetails
                join ii in _context.ItemIngredients on od.ItemId equals ii.ItemId
                join i in _context.Inventories on ii.IngredientId equals i.IngredientId
                where od.OrderId == orderId
                select new { i, ii.QuantityNeeded, od.Quantity };

            foreach (var item in updateInventoryQuery)
            {
                item.i.CurrentStock -= item.QuantityNeeded * item.Quantity;
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

    // Adjust inventory for the ingredients used in the products
    var updateInventoryQuery = from od in _context.OrderDetails
                                join ii in _context.ItemIngredients on od.ItemId equals ii.ItemId
                                join i in _context.Inventories on ii.IngredientId equals i.IngredientId
                                where od.OrderId == orderId
                                select new { i, ii.QuantityNeeded, od.Quantity };

    foreach (var item in updateInventoryQuery)
    {
        item.i.CurrentStock -= item.QuantityNeeded * item.Quantity;
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

        // Get all orders for all users
        public async Task<List<UserOrderDTO>> GetAllOrdersAsync()
        {
            var allOrders = await (from o in _context.Orders
                join pyt in _context.Payments on o.OrderId equals pyt.OrderId into paymentGroup
                from pyt in paymentGroup.DefaultIfEmpty()
                orderby o.OrderDate descending
                select new UserOrderDTO
                {
                    UserId = o.UserId,
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                    PaymentStatus = pyt != null ? pyt.PaymentStatus : "Pending",
                    OrderStatus = o.OrderStatus,
                    TotalCost = o.TotalCost,
                    ShippingInfo = _context.ShippingOptions
                               .FirstOrDefault(si => si.ShippingOptionId == o.ShippingOptionId).ShippingOptionName.ToString(),
                    CustomerName = _context.Users.FirstOrDefault(u => u.UserId == o.UserId).FirstName.ToString(),
                }).ToListAsync();
            return allOrders;
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

        public async Task<UserOrderDetailDTO> GetOrderDetailsAsync(int orderId)
        {
            // First, retrieve the order details.
            var userOrderDetail = await (from o in _context.Orders
                join shc in _context.ShippingOptions on o.ShippingOptionId equals shc.ShippingOptionId into shcGroup
                from shc in shcGroup.DefaultIfEmpty()
                where o.OrderId == orderId
                select new UserOrderDetailDTO()
                {
                    OrderStatus = o.OrderStatus,
                    TotalCost = o.TotalCost,
                    OrderDate = o.OrderDate.ToString("dd/MM/yyyy"),
                    UserId = o.UserId,
                    ShippingOptionId = Convert.ToInt32(o.ShippingOptionId),
                    ShippingCost = shc.Cost,
                    GST = (o.TotalCost * (decimal)0.15),
                    OrderId = o.OrderId
                }).FirstOrDefaultAsync();

            if (userOrderDetail == null)
            {
                return null;
            }

            // Retrieve order item details.
            var orderItems = await (from od in _context.OrderDetails
                join p in _context.Items on od.ItemId equals p.ItemId
                where od.OrderId == orderId
                select new OrderDetailDTO
                {
                    ItemName = p.ItemName,
                    Price = od.PurchasePrice,
                    Quantity = od.Quantity,
                    OrderId = od.OrderId,
                    PurchasePrice = od.PurchasePrice,
                    ItemId = od.ItemId
                }).ToListAsync();

            // Retrieve user address.
            var userAddress = await (from ua in _context.UserAddresses
                where ua.UserId == userOrderDetail.UserId
                select new UserAddressDTO
                {
                    StreetAddress1 = ua.StreetAddress1,
                    StreetAddress2 = ua.StreetAddress2,
                    City = ua.City,
                    PostalCode = ua.PostalCode,
                    StateOrProvince = ua.StateOrProvince
                }).FirstOrDefaultAsync();

            // Retrieve customer contact info.
            var contactInfo = await (from u in _context.Users
                where u.UserId == userOrderDetail.UserId
                select new ContactInfoDTO
                {
                    Name = u.Title + " " + u.FirstName + " " + u.LastName,
                    Email = u.Email,
                    Phone = u.Phone
                }).FirstOrDefaultAsync();

            // Return the combined order details, user address, and contact info.
            userOrderDetail.OrderItems = orderItems;
            userOrderDetail.ShippingAddress = userAddress;
            userOrderDetail.ContactInfo = contactInfo;
            return userOrderDetail;

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
        
        // Get Inventory details for a specific product by aggregating ingredient stocks
        public async Task<ProductInventoryDTO> GetInventoryForProductAsync(int itemId)
        {
            var inventory = await (from ii in _context.ItemIngredients
                join inv in _context.Inventories on ii.IngredientId equals inv.IngredientId
                where ii.ItemId == itemId
                group new { ii, inv } by ii.ItemId into inventoryGroup
                select new ProductInventoryDTO
                {
                    ItemId = inventoryGroup.Key,
                    AvailableStock = inventoryGroup.Min(g => g.inv.CurrentStock / g.ii.QuantityNeeded)
                }).FirstOrDefaultAsync();

            if (inventory == null)
            {
                throw new Exception($"Inventory not found for product with ID: {itemId}");
            }

            return inventory;
        }

        public async Task<bool> UpdateOrderAsync(int orderId, UserOrderDetailDTO updatedOrderDetails)
        {
            // Begin transaction to ensure atomic updates
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 1. Update Order Info
                    var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
                    if (order == null)
                    {
                        return false; // Order not found
                    }

                    order.OrderStatus = updatedOrderDetails.OrderStatus;
                    order.TotalCost = updatedOrderDetails.TotalCost;
                    order.ShippingOptionId = updatedOrderDetails.ShippingOptionId;

                    _context.Orders.Update(order);

                    // 2. Update User Address
                    var userAddress =
                        await _context.UserAddresses.FirstOrDefaultAsync(ua => ua.UserId == updatedOrderDetails.UserId);
                    if (userAddress != null)
                    {
                        userAddress.StreetAddress1 = updatedOrderDetails.ShippingAddress.StreetAddress1;
                        userAddress.StreetAddress2 = updatedOrderDetails.ShippingAddress.StreetAddress2;
                        userAddress.City = updatedOrderDetails.ShippingAddress.City;
                        userAddress.StateOrProvince = updatedOrderDetails.ShippingAddress.StateOrProvince;
                        userAddress.PostalCode = updatedOrderDetails.ShippingAddress.PostalCode;

                        _context.UserAddresses.Update(userAddress);
                    }

                    // 3. Update Contact Info
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == updatedOrderDetails.UserId);
                    if (user != null)
                    {
                        user.Email = updatedOrderDetails.ContactInfo.Email;
                        user.Phone = updatedOrderDetails.ContactInfo.Phone;
                        // You can also update FirstName, LastName if needed, but they are derived in the DTO.
                        _context.Users.Update(user);
                    }

                    // 4. Update Order Items
                    var existingOrderDetails =
                        await _context.OrderDetails.Where(od => od.OrderId == orderId).ToListAsync();

                    foreach (var item in updatedOrderDetails.OrderItems)
                    {
                        var orderDetail = existingOrderDetails.FirstOrDefault(od => od.ItemId == item.ItemId);
                        if (orderDetail != null)
                        {
                            // Update existing item
                            orderDetail.Quantity = item.Quantity;
                            orderDetail.PurchasePrice = item.PurchasePrice;
                            _context.OrderDetails.Update(orderDetail);
                        }
                        else
                        {
                            // Add new item if not exists
                            var newOrderDetail = new OrderDetail
                            {
                                OrderId = orderId,
                                ItemId = item.ItemId,
                                Quantity = item.Quantity,
                                PurchasePrice = item.PurchasePrice
                            };
                            await _context.OrderDetails.AddAsync(newOrderDetail);
                        }
                    }

                    // Save all changes
                    await _context.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();

                    return true; // Update successful
                }
                catch (Exception ex)
                {
                    // Rollback transaction in case of error
                    await transaction.RollbackAsync();
                    Console.WriteLine("Error updating order: " + ex.Message);
                    return false;
                }
            }
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
