using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class PromotionItem
{
    public int PromotionItemId { get; set; }

    public int ItemId { get; set; }

    public decimal? DiscountRate { get; set; }

    public virtual Product Item { get; set; } = null!;
}
