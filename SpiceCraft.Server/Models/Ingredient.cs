using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string IngredientName { get; set; } = null!;

    public string Unit { get; set; } = null!;

    public int ItemsPerUnit { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Inventory? Inventory { get; set; }

    public virtual ICollection<ItemIngredient> ItemIngredients { get; set; } = new List<ItemIngredient>();
}
