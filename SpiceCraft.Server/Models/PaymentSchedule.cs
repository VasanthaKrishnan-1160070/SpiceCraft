using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class PaymentSchedule
{
    public int ScheduleId { get; set; }

    public int CorporateId { get; set; }

    public string ScheduleType { get; set; } = null!;

    public DateOnly? LastPaymentDate { get; set; }

    public DateOnly? NextPaymentDue { get; set; }

    public virtual CorporateClient Corporate { get; set; } = null!;
}
