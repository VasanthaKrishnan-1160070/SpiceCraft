using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class ShippingOption
{
    public int ShippingOptionId { get; set; }

    public string ShippingOptionName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
