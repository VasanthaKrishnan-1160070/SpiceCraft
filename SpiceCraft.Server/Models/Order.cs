using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public int? ShippingOptionId { get; set; }

    public decimal TotalCost { get; set; }

    public bool? IsFreeShipping { get; set; }

    public string? Preference { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ShippingOption? ShippingOption { get; set; }

    public virtual User User { get; set; } = null!;
}
