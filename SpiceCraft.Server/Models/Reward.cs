using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Reward
{
    public int RewardId { get; set; }

    public int PointsThreshold { get; set; }

    public string? RewardDescription { get; set; }

    public virtual ICollection<CustomerReward> CustomerRewards { get; set; } = new List<CustomerReward>();
}
