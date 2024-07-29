using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class ItemCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public int? ParentCategoryId { get; set; }

    public virtual ICollection<ItemCategory> InverseParentCategory { get; set; } = new List<ItemCategory>();

    public virtual ItemCategory? ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual PromotionCategory? PromotionCategory { get; set; }
}
