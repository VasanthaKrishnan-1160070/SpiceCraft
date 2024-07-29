using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class EnquiryType
{
    public int EnquiryTypeId { get; set; }

    public string EnquiryName { get; set; } = null!;

    public virtual ICollection<Enquiry> Enquiries { get; set; } = new List<Enquiry>();
}
