using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;

namespace SchoolManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolFeeController : ControllerBase
    {
        private readonly ISchoolFeeService _schoolFeeService;

        public SchoolFeeController(ISchoolFeeService schoolFeeService)
        {
            _schoolFeeService = schoolFeeService;
        }


        [HttpPost("set-schoolFee")]
        public async Task<IActionResult> SetSchoolFee(ClassFeeDto classFee)
        {
            await _schoolFeeService.SetSchoolFee(classFee);
            return Ok();
        }


        [HttpGet("view-schoolFees")]
        public IActionResult ViewSchoolFees()
        {
            var classFees = _schoolFeeService.ViewSchoolFees();
            return Ok(classFees);
        }


        [HttpPut("edit-schoolFee/{feeId}")]
        public async Task<IActionResult> EditSchoolFee(int feeId, ClassFeeDto updatedFee)
        {
            var result = await _schoolFeeService.EditSchoolFee(feeId, updatedFee);
            if (result)
            {
                return Ok();
            }
            return NotFound();
        }
    }
}
