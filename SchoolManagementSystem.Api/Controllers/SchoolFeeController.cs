using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolFeeController : ControllerBase
    {
        private readonly ISchoolFeeService _schoolFeeService;
        private readonly ILogger<SchoolFeeController> _logger;
        
        public SchoolFeeController(ISchoolFeeService schoolFeeService, ILogger<SchoolFeeController> logger)
        {
            _schoolFeeService = schoolFeeService;
            _logger = logger;
        }


        [HttpPost("add")]
        [SwaggerOperation(Summary = "add school fee", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> AddSchoolFee(SchoolFeeDto schoolFeeDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _schoolFeeService.AddSchoolFee(schoolFeeDto);
            _logger.LogInformation("School fee added successfully.");

            return Ok();
        }


        [HttpPut("update")]
        [SwaggerOperation(Summary = "update school fee.", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> UpdateSchoolFee(int id, SchoolFeeDto schoolFeeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var updatedSchoolFee = await _schoolFeeService.UpdateSchoolFee(id, schoolFeeDto);
            _logger.LogInformation("School fee updated successfully.");

            return Ok(updatedSchoolFee);
        }


        [HttpGet("get-all-schoolFees")]
        [SwaggerOperation(Summary = "get all school fees.", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of all school fees.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetAllSchoolFees()
        {
            var schoolFees = await _schoolFeeService.GetAllSchoolFees();
            if(schoolFees == null)
            {
                return BadRequest();
            }
            _logger.LogInformation("Retrieved all school fees.");

            return Ok(schoolFees);
        }


        [HttpDelete("delete")]
        [SwaggerOperation(Summary = "delete school fee.", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> DeleteSchoolFee(int id)
        {
           await _schoolFeeService.DeleteSchoolFee(id);
            _logger.LogInformation("School fee deleted successfully.");

            return Ok();
        }


        [HttpGet("get-total")]
        [SwaggerOperation(Summary = "get total school fees.", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the total school fees amount")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetTotalSchoolFees()
        {
            var totalFee = await _schoolFeeService.GetTotalSchoolFees();
            if (totalFee < 0)
            {
                return BadRequest();
            }
            _logger.LogInformation("Retrieved total school fees.");

            return Ok(totalFee);
        }


        [HttpGet("get-class-total")]
        [SwaggerOperation(Summary = "get total school fees of a class.", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns total school fee amount of a class")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetTotalSchoolFeesOfClass(string className)
        {
            var totalFeeOfClass = await _schoolFeeService.GetTotalSchoolFeesOfClass(className);

            if (totalFeeOfClass < 0)
            {
                return BadRequest();
            }
            _logger.LogInformation($"Retrieved total school fees of class {className}.");

            return Ok(totalFeeOfClass);
        }


        [HttpGet("get-all-class-fees")]
        [SwaggerOperation(Summary = "get all school fees of a class.", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of school fees of a class")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetAllSchoolFeesOfClass(string className)
        {
            var schoolFeesOfClass = await _schoolFeeService.GetAllSchoolFeesOfClass(className);

            if (schoolFeesOfClass == null)
            {
                return BadRequest();
            }
            _logger.LogInformation($"Retrieved all school fees of class {className}.");

            return Ok(schoolFeesOfClass);
        }
    }
}
