using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }


        [HttpPost("add-subject")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddSubjectAsync([FromBody] SubjectDto addSubject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subjectService.AddSubjectAsync(addSubject);

            return Ok(result);

        }


        [HttpPut("update-subject/{Id}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateSubjectAsync(int Id, [FromBody] SubjectDto updateSubject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subjectService.UpdateSubjectAsync(Id, updateSubject);

            return Ok(result);
        }

        [HttpGet("view-all-subjects")]
        public async Task<IActionResult> ViewAllSubjectsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _subjectService.ViewAllSubjectsAsync();
            return Ok(result);
        }

        [HttpDelete("delete-subject/{Id}")]
        public async Task<IActionResult> DeleteSubjectAsync(int Id)
        {
            var response = await _subjectService.DeleteSubjectAsync(Id);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
