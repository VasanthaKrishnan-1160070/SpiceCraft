using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class UserItemRating
{
    public int UserItemRating1 { get; set; }

    public int UserId { get; set; }

    public int? Rating { get; set; }

    public int ItemId { get; set; }

    public string? RatingDescription { get; set; }

    public string? ImprovementDescription { get; set; }

    public bool? IsNegativeReview { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
