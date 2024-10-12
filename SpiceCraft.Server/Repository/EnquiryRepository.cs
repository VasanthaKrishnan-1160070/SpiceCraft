using Microsoft.EntityFrameworkCore;
using SpiceCraft.Server.Context;
using SpiceCraft.Server.DTO.Enquiry;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.Repository.Interface;

namespace SpiceCraft.Server.Repository
{
    public class EnquiryRepository : IEnquiryRepository
    {
        SpiceCraftContext _context;
        public EnquiryRepository(SpiceCraftContext context) 
        { 
            _context = context;
        }

        public IEnumerable<EnquiryDTO> GetEnquiriesByUser(int userId)
        {
            var enquiries = (from e in _context.Enquiries
                             join m in _context.Messages on e.EnquiryId equals m.EnquiryId
                             join et in _context.EnquiryTypes on e.EnquiryTypeId equals et.EnquiryTypeId
                             where m.SenderUserId == userId || m.ReceiverUserId == userId
                             orderby e.CreatedAt descending
                             select new EnquiryDTO
                             {
                                 EnquiryId = e.EnquiryId,
                                 Regarding = et.EnquiryName,
                                 RequiresReply = _context.Messages
                                    .Where(msg => msg.EnquiryId == e.EnquiryId)
                                    .OrderByDescending(msg => msg.CreatedAt)
                                    .Select(msg => _context.Users
                                        .Where(u => u.UserId == msg.SenderUserId)
                                        .Select(u => u.RoleId)
                                        .FirstOrDefault() == 4 ? "No" : "Yes")
                                    .FirstOrDefault() ?? "No",
                                 Sender = _context.Users
                                     .Where(u => u.UserId == m.SenderUserId)
                                     .Select(u => u.FirstName + " " + u.LastName)
                                     .FirstOrDefault() ?? "",
                                 EnquiryDate = e.CreatedAt.HasValue ? e.CreatedAt.Value.ToString("dd'/'MM'/'yyyy") : string.Empty
                             }).Distinct().ToList();

            return enquiries;
        }

        public InternalEnquiryDTO GetEnquiryByForInternalUser(int userId)
        {
            var enquiries = (from e in _context.Enquiries
                             join m in _context.Messages on e.EnquiryId equals m.EnquiryId
                             join et in _context.EnquiryTypes on e.EnquiryTypeId equals et.EnquiryTypeId
                             orderby e.CreatedAt descending
                             select new EnquiryDTO
                             {
                                 EnquiryId = e.EnquiryId,
                                 Regarding = et.EnquiryName,
                                 RequiresReply = _context.Messages
                                     .Where(msg => msg.EnquiryId == e.EnquiryId)
                                     .OrderByDescending(msg => msg.CreatedAt)
                                     .Select(msg => _context.Users
                                         .Where(u => u.UserId == msg.SenderUserId)
                                         .Select(u => u.RoleId)
                                         .FirstOrDefault() == 4 ? "Yes" : "No")
                                     .FirstOrDefault() ?? "No",
                                 Sender = _context.Users
                                     .Where(u => u.UserId == m.SenderUserId)
                                     .Select(u => u.FirstName + " " + u.LastName)
                                     .FirstOrDefault() ?? "",
                                 EnquiryDate = e.CreatedAt.HasValue ? e.CreatedAt.Value.ToString("dd'/'MM'/'yyyy") : string.Empty
                             }).Distinct().ToList();

            var customers = _context.Users
                .Where(u => u.RoleId == 4)
                .Select(u => new CustomerDTO
                {
                    UserId = u.UserId,
                    Name = u.FirstName + " " + u.LastName + ", Customer Id = (" + u.UserId + ")"
                }).ToList();

            return new InternalEnquiryDTO
            {
                Enquiries = enquiries,
                Customers = customers
            };
        }

        public MessageDTO GetLatestMessageByEnquiryId(int enquiryId)
        {
            var latestMessage = (from m in _context.Messages
                                 join e in _context.Enquiries on m.EnquiryId equals e.EnquiryId
                                 join et in _context.EnquiryTypes on e.EnquiryTypeId equals et.EnquiryTypeId
                                 join s in _context.Users on m.SenderUserId equals s.UserId
                                 join r in _context.Users on m.ReceiverUserId equals r.UserId
                                 where m.EnquiryId == enquiryId
                                 orderby m.CreatedAt descending
                                 select new MessageDTO
                                 {
                                     Regarding = et.EnquiryName,
                                     EnquiryId = e.EnquiryId,
                                     MessageContent = m.MessageContent,
                                     SenderUserId = m.SenderUserId,
                                     ReceiverUserId = m.ReceiverUserId,
                                     MessageDate = m.CreatedAt,
                                     Receiver = r.FirstName + " " + r.LastName,
                                     Sender = s.FirstName + " " + s.LastName,
                                     EnquiryTypeId = et.EnquiryTypeId,
                                     Subject = m.Subject?? et.EnquiryName
                                 }).FirstOrDefault();

            return latestMessage;
        }

        public EnquiryMessagesDTO GetEnquiryMessagesByEnquiryId(int enquiryId)
        {
            var enquiryMessages = (from m in _context.Messages
                                   join e in _context.Enquiries on m.EnquiryId equals e.EnquiryId
                                   join et in _context.EnquiryTypes on e.EnquiryTypeId equals et.EnquiryTypeId
                                   join s in _context.Users on m.SenderUserId equals s.UserId
                                   join r in _context.Users on m.ReceiverUserId equals r.UserId
                                   where m.EnquiryId == enquiryId
                                   orderby m.CreatedAt ascending
                                   select new MessageDTO
                                   {
                                       Regarding = et.EnquiryName,
                                       EnquiryId = e.EnquiryId,
                                       MessageContent = m.MessageContent,
                                       SenderUserId = m.SenderUserId,
                                       ReceiverUserId = m.ReceiverUserId,
                                       MessageDate = m.CreatedAt,
                                       Receiver = r.FirstName + " " + r.LastName,
                                       Sender = s.FirstName + " " + s.LastName,
                                       EnquiryTypeId = et.EnquiryTypeId,
                                       Subject = m.Subject?? et.EnquiryName
                                   }).ToList();

            var latestMessage = GetLatestMessageByEnquiryId(enquiryId);

            return new EnquiryMessagesDTO
            {
                EnquiryMessages = enquiryMessages,
                LatestMessage = latestMessage
            };
        }

        // This method accepts the DTO and maps it to entities internally
        public int CreateEnquiry(EnquiryCreationDTO enquiryDto)
        {
            // Map DTO to Enquiry entity
            var enquiry = new Enquiry
            {
                CreatedAt = DateTime.Now,
                EnquiryTypeId = enquiryDto.EnquiryTypeId,
                // Add any other necessary fields from the DTO...
            };

            // Save the enquiry to the database
            _context.Enquiries.Add(enquiry);
            _context.SaveChanges();

            // Get the newly created enquiry ID
            int enquiryId = enquiry.EnquiryId;

            // Map DTO to Message entity
            var message = new Message
            {
                EnquiryId = enquiryId,
                MessageContent = enquiryDto.InitialMessage,
                SenderUserId = enquiryDto.SenderUserId,
                ReceiverUserId = enquiryDto.ReceiverUserId,
                CreatedAt = DateTime.Now
            };

            // Save the message to the database
            _context.Messages.Add(message);
            _context.SaveChanges();

            return enquiryId;  // Return the newly created enquiry ID
        }


        // This method accepts the DTO and maps it to the entity internally
        public void CreateMessage(MessageDTO messageDto)
        {
            // Map the DTO to the Message entity
            var message = new Message
            {
                EnquiryId = messageDto.EnquiryId,
                MessageContent = messageDto.MessageContent,
                SenderUserId = messageDto.SenderUserId,
                ReceiverUserId = messageDto.ReceiverUserId,
                CreatedAt = DateTime.Now
            };

            // Save the message to the database
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        public MessageDTO GetMessageByMessageId(int messageId)
        {
            var message = (from m in _context.Messages
                           join s in _context.Users on m.SenderUserId equals s.UserId
                           join r in _context.Users on m.ReceiverUserId equals r.UserId
                           where m.MessageId == messageId
                           select new MessageDTO
                           {
                               MessageId = m.MessageId,
                               EnquiryId = m.EnquiryId,
                               Subject = m.Subject,
                               SenderUserId = m.SenderUserId,
                               ReceiverUserId = m.ReceiverUserId,
                               MessageContent = m.MessageContent,
                               MessageDate = m.CreatedAt,
                               Receiver = r.FirstName + " " + r.LastName,
                               Sender = s.FirstName + " " + s.LastName
                           }).FirstOrDefault();

            return message;
        }

        public async Task<IEnumerable<EnquiryTypeDTO>> GetEnquiryTypes()
        {
            return await _context.EnquiryTypes.Select(s => new EnquiryTypeDTO()
            {
                EnquiryTypeId = s.EnquiryTypeId,
                EnquiryName = s.EnquiryName
            }).ToListAsync();
        }
    }
}
