using System;
using System.Collections.Generic;

namespace SpiceCraft.Server.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int SenderUserId { get; set; }

    public int? ReceiverUserId { get; set; }

    public int EnquiryId { get; set; }

    public string? Subject { get; set; }

    public string? MessageContent { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Enquiry Enquiry { get; set; } = null!;

    public virtual User? ReceiverUser { get; set; }

    public virtual User SenderUser { get; set; } = null!;
}
