using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class RecentlyViewed
{
    public int RecentlyViewedId { get; set; }

    public int UserId { get; set; }

    public int ItemId { get; set; }

    public int ViewCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
