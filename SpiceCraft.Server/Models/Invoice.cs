using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Invoice
{
    public int InvoiceId { get; set; }

    public int OrderId { get; set; }

    public DateOnly IssueDate { get; set; }

    public DateOnly DueDate { get; set; }

    public decimal TotalAmount { get; set; }

    public bool? Paid { get; set; }

    public virtual Order Order { get; set; } = null!;
}
