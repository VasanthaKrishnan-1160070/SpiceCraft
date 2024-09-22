using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class OrderDetail
{
    public int OrderDetailsId { get; set; }

    public int OrderId { get; set; }

    public int ItemId { get; set; }

    public decimal ActualPrice { get; set; }

    public string? Description { get; set; }

    public decimal DiscountRate { get; set; }

    public int Quantity { get; set; }

    public decimal PurchasePrice { get; set; }

    public string? SpiceLevel { get; set; }

    public string? Size { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
