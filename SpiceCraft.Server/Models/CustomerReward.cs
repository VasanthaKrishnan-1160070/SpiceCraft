using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class CustomerReward
{
    public int CustomerRewardsId { get; set; }

    public int RewardId { get; set; }

    public int UserId { get; set; }

    public bool? IsUsed { get; set; }

    public virtual Reward Reward { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
