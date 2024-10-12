using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class ItemIngredient
{
    public int ItemIngredientId { get; set; }

    public int ItemId { get; set; }

    public int IngredientId { get; set; }

    public string? Size { get; set; }

    public int QuantityNeeded { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
