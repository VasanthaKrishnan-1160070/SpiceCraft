using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ItemId { get; set; }

    public int CurrentStock { get; set; }

    public int LowStockThreshold { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product Item { get; set; } = null!;
}
