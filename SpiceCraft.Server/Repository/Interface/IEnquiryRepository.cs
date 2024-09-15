using SpiceCraft.Server.DTO.Enquiry;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.Repository.Interface
{
    public interface IEnquiryRepository
    {
        IEnumerable<EnquiryDTO> GetEnquiriesByUser(int userId);
        InternalEnquiryDTO GetEnquiryByForInternalUser(int userId);
        MessageDTO GetLatestMessageByEnquiryId(int enquiryId);
        EnquiryMessagesDTO GetEnquiryMessagesByEnquiryId(int enquiryId);
        int CreateEnquiry(EnquiryCreationDTO enquiryDto);
        void CreateMessage(MessageDTO messageDto);
        MessageDTO GetMessageByMessageId(int messageId);
        Task<IEnumerable<EnquiryTypeDTO>> GetEnquiryTypes();
    }
}
