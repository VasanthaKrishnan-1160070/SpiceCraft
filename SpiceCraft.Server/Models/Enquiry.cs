using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Enquiry
{
    public int EnquiryId { get; set; }

    public int EnquiryTypeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual EnquiryType EnquiryType { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
