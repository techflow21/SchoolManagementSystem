using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Service.Implementation;

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }


        [HttpPost("add-class")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddClassAsync([FromBody] ClassRequestDto addClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _classService.AddClassAsync(addClass);

            return Ok(result);

        }


        [HttpPut("update-class/{Id}")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UpdateClassAsync(int Id, [FromBody] ClassRequestDto updateClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _classService.UpdateClassAsync(Id, updateClass);

            return Ok(result);
        }

        [HttpGet("view-all-classes")]
        public async Task<IActionResult> ViewAllClassesAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _classService.ViewAllClassesAsync();
            return Ok(result);
        }

        [HttpDelete("delete-class/{Id}")]
        public async Task<IActionResult> DeleteClassAsync(int Id)
        {
            var response = await _classService.DeleteClassAsync(Id);
            if (!response.Success)
            {
                return NotFound(response); 
            }

            return Ok(response);
        }
    }
}
