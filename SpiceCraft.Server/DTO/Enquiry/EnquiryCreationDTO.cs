namespace SpiceCraft.Server.DTO.Enquiry
{
    public class EnquiryCreationDTO
    {
        public int EnquiryTypeId { get; set; }  // Enquiry type
        public string InitialMessage { get; set; }  // Initial message content
        public int SenderUserId { get; set; }  // ID of the sender
        public int? ReceiverUserId { get; set; }  // Optional receiver ID
    }
}
