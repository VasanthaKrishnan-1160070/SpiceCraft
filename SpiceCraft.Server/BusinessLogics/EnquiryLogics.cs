using SpiceCraft.Server.BusinessLogics.Interface;
using SpiceCraft.Server.DTO.Enquiry;
using SpiceCraft.Server.Helpers;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.BusinessLogics
{
    public class EnquiryLogics : IEnquiryLogics
    {
        private readonly IEnquiryRepository _enquiryRepository;

        public EnquiryLogics(IEnquiryRepository enquiryRepository)
        {
            _enquiryRepository = enquiryRepository;
        }

        // Retrieves enquiries for a specific user
        public ResultDetail<IEnumerable<EnquiryDTO>> GetEnquiriesByUser(int userId)
        {
            var enquiries = _enquiryRepository.GetEnquiriesByUser(userId);
            if (enquiries != null && enquiries.Any())
            {
                return HelperFactory.Msg.Success(enquiries);
            }
            return HelperFactory.Msg.Error<IEnumerable<EnquiryDTO>>("No enquiries found.");
        }

        // Retrieves enquiries for internal users
        public ResultDetail<InternalEnquiryDTO> GetEnquiryByForInternalUser(int userId)
        {
            var enquiries = _enquiryRepository.GetEnquiryByForInternalUser(userId);
            if (enquiries != null)
            {
                return HelperFactory.Msg.Success(enquiries);
            }
            return HelperFactory.Msg.Error<InternalEnquiryDTO>("No enquiries found for internal user.");
        }

        // Retrieves the latest message for a specific enquiry ID
        public ResultDetail<MessageDTO> GetLatestMessageByEnquiryId(int enquiryId)
        {
            var latestMessage = _enquiryRepository.GetLatestMessageByEnquiryId(enquiryId);
            if (latestMessage != null)
            {
                return HelperFactory.Msg.Success(latestMessage);
            }
            return HelperFactory.Msg.Error<MessageDTO>("No message found for this enquiry.");
        }

        // Retrieves all messages for a specific enquiry ID
        public ResultDetail<EnquiryMessagesDTO> GetEnquiryMessagesByEnquiryId(int enquiryId)
        {
            var enquiryMessages = _enquiryRepository.GetEnquiryMessagesByEnquiryId(enquiryId);
            if (enquiryMessages != null && enquiryMessages.EnquiryMessages.Any())
            {
                return HelperFactory.Msg.Success(enquiryMessages);
            }
            return HelperFactory.Msg.Error<EnquiryMessagesDTO>("No messages found for this enquiry.");
        }

        // Creates a new enquiry and the first message associated with it
        public ResultDetail<int> CreateEnquiry(EnquiryCreationDTO enquiryDto)
        {
            try
            {
                // The service layer only deals with DTOs.
                var enquiryId = _enquiryRepository.CreateEnquiry(enquiryDto);
                return HelperFactory.Msg.Success(enquiryId, "Enquiry created successfully.");
            }
            catch (Exception ex)
            {
                return HelperFactory.Msg.Error<int>($"Error creating enquiry: {ex.Message}");
            }
        }

        // Creates a new message associated with an enquiry
        // This method accepts a MessageDTO and passes it to the repository
        public ResultDetail<bool> CreateMessage(MessageDTO messageDto)
        {
            try
            {
                // The service layer only deals with DTOs.
                _enquiryRepository.CreateMessage(messageDto);
                return HelperFactory.Msg.Success(true, "Message created successfully.");
            }
            catch (Exception ex)
            {
                return HelperFactory.Msg.Error<bool>($"Error creating message: {ex.Message}");
            }
        }

        // Retrieves a specific message by its ID
        public ResultDetail<MessageDTO> GetMessageByMessageId(int messageId)
        {
            var message = _enquiryRepository.GetMessageByMessageId(messageId);
            if (message != null)
            {
                return HelperFactory.Msg.Success(message);
            }
            return HelperFactory.Msg.Error<MessageDTO>("No message found for this ID.");
        }

        public async Task<ResultDetail<IEnumerable<EnquiryTypeDTO>>> GetEnquiryTypes()
        {
            var enquiryTypes = await _enquiryRepository.GetEnquiryTypes();
            if (enquiryTypes != null)
            {
                return HelperFactory.Msg.Success(enquiryTypes);
            }
            return HelperFactory.Msg.Error(enquiryTypes, "No Enquiry type found.");
        }
    }
}
