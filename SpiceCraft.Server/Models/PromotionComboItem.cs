using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class PromotionComboItem
{
    public int PromotionComboItemsId { get; set; }

    public int ItemId { get; set; }

    public string? ComboName { get; set; }

    public int BuyQuantity { get; set; }

    public int GetQuantity { get; set; }

    public virtual Product Item { get; set; } = null!;
}
