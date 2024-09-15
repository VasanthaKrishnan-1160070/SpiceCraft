using SpiceCraft.Server.DTO.Enquiry;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Models;

namespace SpiceCraft.Server.BusinessLogics.Interface
{
    public interface IEnquiryLogics
    {
        // Retrieves enquiries for a specific user
        ResultDetail<IEnumerable<EnquiryDTO>> GetEnquiriesByUser(int userId);

        // Retrieves enquiries for internal users
        ResultDetail<InternalEnquiryDTO> GetEnquiryByForInternalUser(int userId);

        // Retrieves the latest message for a specific enquiry ID
        ResultDetail<MessageDTO> GetLatestMessageByEnquiryId(int enquiryId);

        // Retrieves all messages for a specific enquiry ID
        ResultDetail<EnquiryMessagesDTO> GetEnquiryMessagesByEnquiryId(int enquiryId);

        // Creates a new enquiry and the first message associated with it
        ResultDetail<int> CreateEnquiry(EnquiryCreationDTO enquiryDto);

        // Creates a new message associated with an enquiry
        ResultDetail<bool> CreateMessage(MessageDTO messageDto);

        // Retrieves a specific message by its ID
        ResultDetail<MessageDTO> GetMessageByMessageId(int messageId);

        Task<ResultDetail<IEnumerable<EnquiryTypeDTO>>> GetEnquiryTypes();
    }
}
