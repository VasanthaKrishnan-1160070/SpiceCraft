using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class CartItem
{
    public int CartItemId { get; set; }

    public int CartId { get; set; }

    public int ItemId { get; set; }

    public int Quantity { get; set; }

    public string? Size { get; set; }

    public string? Color { get; set; }

    public decimal? PriceAtAdd { get; set; }

    public virtual ShoppingCart Cart { get; set; } = null!;

    public virtual Product Item { get; set; } = null!;
}
