using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class CustomerGiftCard
{
    public int CustomerGiftCardId { get; set; }

    public int UserId { get; set; }

    public int GiftCardId { get; set; }

    public virtual GiftCard GiftCard { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
