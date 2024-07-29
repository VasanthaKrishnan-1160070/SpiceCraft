using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Notification
{
    public int NotificationId { get; set; }

    public string? NotificationType { get; set; }

    public int EntityId { get; set; }

    public bool? IsRead { get; set; }

    public string Title { get; set; } = null!;

    public string? Message { get; set; }

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
