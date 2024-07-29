using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class ItemImage
{
    public int ImageId { get; set; }

    public int ItemId { get; set; }

    public string ImageCode { get; set; } = null!;

    public string? ImageName { get; set; }

    public int? ImageIndex { get; set; }

    public bool? IsMain { get; set; }

    public virtual Product Item { get; set; } = null!;
}
