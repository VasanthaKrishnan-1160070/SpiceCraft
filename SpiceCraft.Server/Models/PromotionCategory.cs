using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class PromotionCategory
{
    public int PromotionCategoryId { get; set; }

    public int CategoryId { get; set; }

    public decimal DiscountRate { get; set; }

    public virtual ItemCategory Category { get; set; } = null!;
}
