using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class CorporateClient
{
    public int CorporateId { get; set; }

    public int UserId { get; set; }

    public bool? Approved { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? CompanyDescription { get; set; }

    public decimal? DiscountRate { get; set; }

    public decimal? CreditLimit { get; set; }

    public decimal? CreditUsed { get; set; }

    public string PaymentOption { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<PaymentSchedule> PaymentSchedules { get; set; } = new List<PaymentSchedule>();

    public virtual User User { get; set; } = null!;
}
