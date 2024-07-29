using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Title { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? ProfileImg { get; set; }

    public int RoleId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CustomerGiftCard> CustomerGiftCards { get; set; } = new List<CustomerGiftCard>();

    public virtual ICollection<CustomerReward> CustomerRewards { get; set; } = new List<CustomerReward>();

    public virtual ICollection<Message> MessageReceiverUsers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenderUsers { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Role Role { get; set; } = null!;

    public virtual ShoppingCart? ShoppingCart { get; set; }

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

    public virtual UsersCredential? UsersCredential { get; set; }
}
