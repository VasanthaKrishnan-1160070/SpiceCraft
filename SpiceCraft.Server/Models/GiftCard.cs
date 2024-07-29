using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class GiftCard
{
    public int GiftCardId { get; set; }

    public string Code { get; set; } = null!;

    public decimal Balance { get; set; }

    public DateOnly? ExpirationDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CustomerGiftCard> CustomerGiftCards { get; set; } = new List<CustomerGiftCard>();
}
