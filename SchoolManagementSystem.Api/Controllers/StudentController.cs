using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("add-student")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddStudentAsync([FromForm] StudentRequestDto addStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentService.AddStudentAsync(addStudent);

            return Ok(result);

        }

        [HttpPut("update-student/{Id}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateStudentAsync(int Id, [FromForm] StudentRequestDto updateStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentService.UpdateStudentAsync(Id, updateStudent);

            return Ok(result);
        }

        [HttpGet("view-all-students")]
        public async Task<IActionResult> ViewAllStudentsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentService.ViewAllStudentsAsync();

            return Ok(result);
        }

        [HttpGet("view-student/{Id}")]
        public async Task<IActionResult> ViewStudentAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentService.ViewStudentAsync(Id);

            return Ok(result);
        }

        [HttpPut("deactivate-student/{Id}")]
        public async Task<IActionResult> DeactivateStudentAsync(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _studentService.DeactivateStudentAsync(Id);

            return Ok(result);
        }
        
    }
}
