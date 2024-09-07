using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public decimal? PriceAtAdd { get; set; }

    public virtual ShoppingCart Cart { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
