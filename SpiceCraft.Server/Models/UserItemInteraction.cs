using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class UserItemInteraction
{
    public int UserItemInteractionId { get; set; }

    public int UserId { get; set; }

    public int? Interaction { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
