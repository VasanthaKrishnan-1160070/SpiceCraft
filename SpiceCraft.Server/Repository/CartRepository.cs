using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Cart;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository;

public class CartRepository : ICartRepository
{
    private readonly SpiceCraftContext _context;

    public CartRepository(SpiceCraftContext context)
    {
        _context = context;
    }

     // Retrieves all shopping cart items for a given user ID
     public async Task<ShoppingCartDTO> GetShoppingCartsByUserIdAsync(int userId)
{
    var cartItems = await (from sc in _context.ShoppingCarts
                           join ci in _context.CartItems on sc.CartId equals ci.CartId
                           join p in _context.Items on ci.ItemId equals p.ItemId
                           join pp in _context.PromotionItems on p.ItemId equals pp.ItemId into ppGroup
                           from pp in ppGroup.DefaultIfEmpty()
                           join prc in _context.PromotionCategories on p.CategoryId equals prc.CategoryId into prcGroup
                           from prc in prcGroup.DefaultIfEmpty()
                           where sc.UserId == userId && sc.IsOrdered == false
                           select new
                           {
                               p.ItemName,
                               p.Price,
                               ci.Quantity,
                               ppDiscountRate = pp != null ? pp.DiscountRate : (decimal?)null,
                               prcDiscountRate = prc != null ? prc.DiscountRate : (decimal?)null,
                               ci.CartItemId,
                               sc.CartId,
                               p.ItemId
                           }).ToListAsync();

    var cartItemDTOs = cartItems.Select(cartItem => new CartItemDTO
    {
        ItemName = cartItem.ItemName,
        UnitPrice = cartItem.Price.ToString("C"),
        Quantity = cartItem.Quantity,
        ActualPrice = (cartItem.Price * cartItem.Quantity).ToString("C"),
        DiscountRate = ((cartItem.ppDiscountRate ?? cartItem.prcDiscountRate ?? 0) / 100).ToString("P"), // String formatting in C#
        FinalPrice = (cartItem.Price * cartItem.Quantity * (1 - (cartItem.ppDiscountRate ?? cartItem.prcDiscountRate ?? 0) / 100)).ToString("C"),
        CartItemId = cartItem.CartItemId,
        CartId = cartItem.CartId,
        ItemId = cartItem.ItemId
    }).ToList();

    var totalPrice = await GetTotalCartPrice(userId);

    return new ShoppingCartDTO
    {
        CartItems = cartItemDTOs,
        TotalPrice = totalPrice
    };
}

    // Retrieves all shopping cart items for a corporate client
    public async Task<ShoppingCartDTO> GetShoppingCartsByUserIdForCorporateClientsAsync(int userId)
    {
        var cartItems = await (from sc in _context.ShoppingCarts
                               join ci in _context.CartItems on sc.CartId equals ci.CartId
                               join p in _context.Items on ci.ItemId equals p.ItemId
                               join pp in _context.PromotionBulkItems on p.ItemId equals pp.ItemId into ppGroup
                               from pp in ppGroup.DefaultIfEmpty()
                               where sc.UserId == userId && sc.IsOrdered == false
                               select new CartItemDTO
                               {
                                   ItemName = p.ItemName,
                                   UnitPrice = p.Price.ToString("C"),
                                   Quantity = ci.Quantity,
                                   ActualPrice = (p.Price * ci.Quantity).ToString("C"),
                                   DiscountRate = pp != null && ci.Quantity >= pp.RequiredQuantity 
                                                ? $"{pp.DiscountRate}%"
                                                : pp != null && ci.Quantity < pp.RequiredQuantity 
                                                    ? $"Needed at least {pp.RequiredQuantity} quantity to get discount of {pp.DiscountRate}%"
                                                    : "No Discount available",
                                   FinalPrice = pp != null && ci.Quantity >= pp.RequiredQuantity 
                                       ? $"{p.Price * ci.Quantity * (1 - pp.DiscountRate / 100):C}" 
                                       : $"{p.Price * ci.Quantity:C}",
                                   CartItemId = ci.CartItemId,
                                   CartId = sc.CartId,
                                   ItemId = p.ItemId
                               }).ToListAsync();

        var totalPrice = await GetTotalCartPriceForCorporateClients(userId);

        return new ShoppingCartDTO
        {
            CartItems = cartItems,
            TotalPrice = totalPrice
        };
    }

    // Retrieves total cart price
    public async Task<decimal> GetTotalCartPrice(int userId)
    {
        var result = await (from sc in _context.ShoppingCarts
            join ci in _context.CartItems on sc.CartId equals ci.CartId
            join p in _context.Items on ci.ItemId equals p.ItemId
            join pp in _context.PromotionItems on p.ItemId equals pp.ItemId into ppGroup
            from pp in ppGroup.DefaultIfEmpty()
            join prc in _context.PromotionCategories on p.CategoryId equals prc.CategoryId into prcGroup
            from prc in prcGroup.DefaultIfEmpty()
            where sc.UserId == userId && sc.IsOrdered == false
            select new
            {
                TotalPrice = p.Price * ci.Quantity * (1 - ((pp != null ? pp.DiscountRate : (prc != null ? prc.DiscountRate : 0)) / 100))
            }).SumAsync(x => x.TotalPrice);

        return result ?? 0;
    }

    // Retrieves total cart price for corporate clients
    public async Task<decimal> GetTotalCartPriceForCorporateClients(int userId)
    {
        var result = await (from sc in _context.ShoppingCarts
                            join ci in _context.CartItems on sc.CartId equals ci.CartId
                            join p in _context.Items on ci.ItemId equals p.ItemId
                            join pp in _context.PromotionBulkItems on p.ItemId equals pp.ItemId into ppGroup
                            from pp in ppGroup.DefaultIfEmpty()
                            where sc.UserId == userId && sc.IsOrdered == false
                            select new
                            {
                                FinalPrice = pp != null && ci.Quantity >= pp.RequiredQuantity
                                            ? p.Price * ci.Quantity * (1 - pp.DiscountRate / 100)
                                            : p.Price * ci.Quantity
                            }).SumAsync(x => x.FinalPrice);

        return result ?? 0;
    }

    // Retrieves a specific cart item by its ID
    public async Task<CartItemDTO> GetCartItemByIdAsync(int cartItemId)
    {
        var cartItem = await (from ci in _context.CartItems
            join p in _context.Items on ci.ItemId equals p.ItemId
            where ci.CartItemId == cartItemId
            select new CartItemDTO
            {
                CartItemId = ci.CartItemId,
                CartId = ci.CartId,
                ItemId = ci.ItemId,
                Quantity = ci.Quantity,
                PriceAtAdd = ci.PriceAtAdd
            }).FirstOrDefaultAsync();

        return cartItem ?? null; // Explicitly return null
    }

    // Retrieves a specific cart item by cart ID and product ID
    public async Task<CartItemDTO> GetCartItemAsync(int cartId, int productId)
    {
        var cartItems =  await (from ci in _context.CartItems
            where ci.CartId == cartId && ci.ItemId == productId
            select new CartItemDTO
            {
                CartItemId = ci.CartItemId,
                CartId = ci.CartId,
                ItemId = ci.ItemId,
                Quantity = ci.Quantity,
                PriceAtAdd = ci.PriceAtAdd
            }).FirstOrDefaultAsync();
        
        return cartItems?? null; // Explicitly return null  
    }

    // Increments the quantity of a specific cart item
    public async Task IncrementCartItemQtyAsync(int cartItemId, int quantity)
    {
        var cartItem = await GetCartItemByIdAsync(cartItemId);
        if (cartItem != null)
        {
            cartItem.Quantity += quantity;
            await _context.SaveChangesAsync();
        }
    }

    // Decrements the quantity of a specific cart item
    public async Task DecrementCartItemQtyAsync(int cartItemId, int quantity)
    {
        var cartItem = await GetCartItemByIdAsync(cartItemId);
        if (cartItem != null && cartItem.Quantity > 1)
        {
            cartItem.Quantity -= quantity;
            await _context.SaveChangesAsync();
        }
    }

    // Creates a new cart item or updates an existing one
    public async Task CreateOrUpdateCartItemAsync(CartItemDTO cartItemDTO)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.CartId == cartItemDTO.CartId && ci.ItemId == cartItemDTO.ItemId);

        if (cartItem != null)
        {
            cartItem.Quantity = cartItemDTO.Quantity;
            cartItem.PriceAtAdd = cartItemDTO.PriceAtAdd;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                CartId = cartItemDTO.CartId,
                ItemId = cartItemDTO.ItemId,
                Quantity = cartItemDTO.Quantity,
                PriceAtAdd = cartItemDTO.PriceAtAdd
            });
        }
        await _context.SaveChangesAsync();
    }

    // Deletes a specific cart item by its ID
    public async Task DeleteCartItemAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem != null)
        {
            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
        }
    }
}

