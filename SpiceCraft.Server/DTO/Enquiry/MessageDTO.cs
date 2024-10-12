namespace SpiceCraft.Server.DTO.Enquiry
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public string Regarding { get; set; }
        public int EnquiryId { get; set; }
        public string MessageContent { get; set; }

        public string Subject { get; set; }
        public int SenderUserId { get; set; }
        public int? ReceiverUserId { get; set; }
        public DateTime? MessageDate { get; set; }
        public string Receiver { get; set; }
        public string Sender { get; set; }
        
        public int EnquiryTypeId { get; set; }
    }
}
