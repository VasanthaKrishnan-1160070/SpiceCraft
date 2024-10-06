using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class UserActivityLog
{
    public int UserActivityLogId { get; set; }

    public int UserId { get; set; }

    public string NavigationItem { get; set; } = null!;

    public string Routing { get; set; } = null!;

    public double? TimeSpent { get; set; }

    public string? SessionId { get; set; }

    public int? ClickCount { get; set; }

    public virtual User User { get; set; } = null!;
}
