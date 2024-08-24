using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class UsersCredential
{
    public int UserCredentialId { get; set; }

    public string UserName { get; set; } = null!;

    public string? Password { get; set; }

    public int UserId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
