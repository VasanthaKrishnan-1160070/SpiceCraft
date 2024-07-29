using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Payment
{
    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public int OrderId { get; set; }

    public string? PaymentMethod { get; set; }

    public decimal Amount { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime PaymentDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
