using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public int CategoryId { get; set; }

    public string ItemName { get; set; } = null!;

    public bool OwnProduct { get; set; }

    public decimal Discount { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public bool IsRemoved { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ItemCategory Category { get; set; } = null!;

    public virtual ICollection<ItemImage> ItemImages { get; set; } = new List<ItemImage>();

    public virtual ICollection<ItemIngredient> ItemIngredients { get; set; } = new List<ItemIngredient>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual PromotionBulkItem? PromotionBulkItem { get; set; }

    public virtual PromotionComboItem? PromotionComboItem { get; set; }

    public virtual PromotionItem? PromotionItem { get; set; }

    public virtual ICollection<RecentlyViewed> RecentlyVieweds { get; set; } = new List<RecentlyViewed>();

    public virtual ICollection<UserItemRating> UserItemRatings { get; set; } = new List<UserItemRating>();
}
