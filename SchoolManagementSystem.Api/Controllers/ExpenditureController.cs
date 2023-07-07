using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace SchoolManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenditureController : ControllerBase
    {
        private readonly IManageExpenditure _expenditureService;

        public ExpenditureController(IManageExpenditure expenditureService)
        {
            _expenditureService = expenditureService;
        }

        [HttpPost]
        [Route("add")]
        [SwaggerOperation(Summary = "add expenditure expenses", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "add expenditures.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The request was invalid")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> AddExpenditureAsync(AddExpenditureDto modelRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _expenditureService.AddExpenditureAsync(modelRequest);
            return Ok(response);
        }

        [HttpGet]
        [Route("all")]
        [SwaggerOperation(Summary = "Get all expenditures", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of all expenditure history", typeof(IEnumerable<ExpenditureHistoryDto>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetAllExpenditureHistoryAsync()
        {           
            var response = await _expenditureService.ViewExpenditureHistoryAsync();
            return Ok(response);
        }

        [HttpPut]
        [Route("update")]
        [SwaggerOperation(Summary = "Update expenditure expenses", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Update expenditures.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The request was invalid")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<(string, EditExpenditureResponseDto)>> UpdateExpenditureAsync([FromBody] EditExpenditureRequestDto modelRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _expenditureService.EditExpenditureAsync(modelRequest);
            return Ok(new {message=response.Item1, expenditure = response.Item2});
        }
    }
}
