using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class PromotionBulkItem
{
    public int PromotionBulkItemId { get; set; }

    public int ItemId { get; set; }

    public int RequiredQuantity { get; set; }

    public decimal? DiscountRate { get; set; }

    public virtual Product Item { get; set; } = null!;
}
