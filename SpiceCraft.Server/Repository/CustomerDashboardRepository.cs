using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository;

public class CustomerDashboardRepository : ICustomerDashboardRepository
{
    private readonly SpiceCraftContext _context;

    public CustomerDashboardRepository(SpiceCraftContext context)
    {
        _context = context;
    }

    // Get total number of orders for the user
    public async Task<int> GetTotalOrdersAsync(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .CountAsync();
    }

    // Get number of shipped orders for the user
    public async Task<int> GetShippedOrdersAsync(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId && o.OrderStatus == "Shipped")
            .CountAsync();
    }

    // Get number of cancelled orders for the user
    public async Task<int> GetCancelledOrdersAsync(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId && o.OrderStatus == "Cancelled")
            .CountAsync();
    }

    // Get the gift card balance for the user
    public async Task<decimal> GetGiftCardBalanceAsync(int userId)
    {
        var giftCard = await _context.CustomerGiftCards
            .Where(cg => cg.UserId == userId)
            .Include(cg => cg.GiftCard)
            .FirstOrDefaultAsync();

        return giftCard?.GiftCard.Balance ?? 0;
    }

    // Get the total number of items in the user's cart
    public async Task<int> GetCartItemsCountAsync(int userId)
    {
        var cart = await _context.ShoppingCarts
            .Where(c => c.UserId == userId)
            .FirstOrDefaultAsync();

        return cart != null
            ? await _context.CartItems
                .Where(ci => ci.CartId == cart.CartId)
                .CountAsync()
            : 0;
    }

    // Get the number of unread notifications for the user
    public async Task<int> GetNotificationCountAsync(int userId)
    {
        return await _context.Notifications
            .Where(n => n.UserId == userId && n.IsRead == false)
            .CountAsync();
    }

}