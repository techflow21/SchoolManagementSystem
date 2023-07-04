using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaffSalaryController : ControllerBase
    {
        private readonly ISalaryService _staffSalaryService;

        public StaffSalaryController(ISalaryService salaryService)
        {
            _staffSalaryService = salaryService;
        }

        [HttpPost("addingStaffSalary")]
        public async Task<IActionResult> AddStaffSalary(AddStaffSalaryDto addStaffSalaryDto)
        {
            var salary = await _staffSalaryService.AddStaffSalaryAsync(addStaffSalaryDto);
            return Ok(salary);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all salaries.", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of all paid salaries.", typeof(IEnumerable<SalaryHistoryDto>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<IEnumerable<SalaryHistoryDto>>> GetAllSalaries()
        {
            var salary = await _staffSalaryService.ViewSalaryHistoryAsync();
            return Ok(salary);
        }

        [HttpPut]
        [SwaggerOperation(Summary = "Update all salaries.", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Update the list of all paid salaries.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<Salary>> UpdateStaffSalary(EditStaffSalaryDto editStaffSalaryDto)
        {
            var salary = await _staffSalaryService.EditStaffSalaryAsync(editStaffSalaryDto);
            return Ok(salary);
        }
    }
}
