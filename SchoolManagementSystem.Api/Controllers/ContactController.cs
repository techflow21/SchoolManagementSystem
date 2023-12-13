using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/contact")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }


        [HttpPost("contact-form")]
        [SwaggerOperation(Summary = "submit contact form", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> SubmitContactForm([FromBody] ContactRequestDto request)
        {
            try
            {
                await _contactService.SubmitContactForm(request);
                return Ok("Contact form submitted successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
