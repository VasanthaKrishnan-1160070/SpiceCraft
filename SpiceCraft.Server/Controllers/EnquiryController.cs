using Microsoft.AspNetCore.Mvc;
using SpiceCraft.Server.DTO.Enquiry;
using SpiceCraft.Server.Models;
using SpiceCraft.Server.BusinessLogics.Interface;

namespace SpiceCraft.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryLogics _enquiryLogics;

        public EnquiryController(IEnquiryLogics enquiryLogics)
        {
            _enquiryLogics = enquiryLogics;
        }

        // GET: api/enquiry/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetEnquiriesByUser(int userId)
        {
            var result = _enquiryLogics.GetEnquiriesByUser(userId);
            return Ok(result);
        }

        // GET: api/enquiry/internal/{userId}
        [HttpGet("internal/{userId}")]
        public IActionResult GetEnquiryByForInternalUser(int userId)
        {
            var result = _enquiryLogics.GetEnquiryByForInternalUser(userId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        // GET: api/enquiry/{enquiryId}/latest-message
        [HttpGet("{enquiryId}/latest-message")]
        public IActionResult GetLatestMessageByEnquiryId(int enquiryId)
        {
            var result = _enquiryLogics.GetLatestMessageByEnquiryId(enquiryId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        // GET: api/enquiry/{enquiryId}/messages
        [HttpGet("{enquiryId}/messages")]
        public IActionResult GetEnquiryMessagesByEnquiryId(int enquiryId)
        {
            var result = _enquiryLogics.GetEnquiryMessagesByEnquiryId(enquiryId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }

        // Get: api/enquiry/enquiry-types
        [HttpGet("enquiry-types")]
        public async Task<IActionResult> GetEnquiryTypes()
        {
            var result = await _enquiryLogics.GetEnquiryTypes();
            return Ok(result);
        }

        // POST: api/enquiry
        [HttpPost]
        public IActionResult CreateEnquiry([FromBody] EnquiryCreationDTO enquiryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _enquiryLogics.CreateEnquiry(enquiryDto);

            if (result.IsSuccess)
            {
                return Ok(result.Data);  // Return the newly created enquiryId
            }
            return BadRequest(result.Message);
        }

        // POST: api/enquiry/message
        [HttpPost("message")]
        public IActionResult CreateMessage([FromBody] MessageDTO messageDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Pass the DTO directly to the business logic
            var result = _enquiryLogics.CreateMessage(messageDto);

            if (result.IsSuccess)
            {
                return Ok(result.Message);  // Success message
            }
            return BadRequest(result.Message);
        }

        // GET: api/enquiry/message/{messageId}
        [HttpGet("message/{messageId}")]
        public IActionResult GetMessageByMessageId(int messageId)
        {
            var result = _enquiryLogics.GetMessageByMessageId(messageId);
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }
            return NotFound(result.Message);
        }
    }
}
