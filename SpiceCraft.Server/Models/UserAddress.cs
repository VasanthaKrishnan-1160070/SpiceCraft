using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class UserAddress
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string? AddressType { get; set; }

    public string StreetAddress1 { get; set; } = null!;

    public string? StreetAddress2 { get; set; }

    public string City { get; set; } = null!;

    public string? StateOrProvince { get; set; }

    public string? PostalCode { get; set; }

    public virtual User User { get; set; } = null!;
}
