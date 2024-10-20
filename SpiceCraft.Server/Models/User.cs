﻿using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Title { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string? ProfileImg { get; set; }

    public DateOnly? DateofBirth { get; set; }

    public int RoleId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual CorporateClient? CorporateClient { get; set; }

    public virtual ICollection<CustomerGiftCard> CustomerGiftCards { get; set; } = new List<CustomerGiftCard>();

    public virtual ICollection<CustomerReward> CustomerRewards { get; set; } = new List<CustomerReward>();

    public virtual ICollection<Message> MessageReceiverUsers { get; set; } = new List<Message>();

    public virtual ICollection<Message> MessageSenderUsers { get; set; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<RecentlyViewed> RecentlyVieweds { get; set; } = new List<RecentlyViewed>();

    public virtual Role Role { get; set; } = null!;

    public virtual ShoppingCart? ShoppingCart { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual ICollection<UserActivityLog> UserActivityLogs { get; set; } = new List<UserActivityLog>();

    public virtual ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();

    public virtual ICollection<UserItemInteraction> UserItemInteractions { get; set; } = new List<UserItemInteraction>();

    public virtual ICollection<UserItemRating> UserItemRatings { get; set; } = new List<UserItemRating>();

    public virtual UsersCredential? UsersCredential { get; set; }
}
