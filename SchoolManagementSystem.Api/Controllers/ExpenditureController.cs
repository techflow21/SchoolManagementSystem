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
        public async Task<IActionResult> AddExpenditureAsync([FromBody] AddExpenditureDto modelRequest)
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

        [HttpGet]
        //[Route("all")]
        [SwaggerOperation(Summary = "Get an expenditure by id", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a single expenditure")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetExpenditureByIdAsync(int expenditureId)
        {
            var response = await _expenditureService.GetExpenditureByIdAsync(expenditureId);
            return Ok(response);
        }

        [HttpGet("search")]        
        [SwaggerOperation(Summary = "Search an expenditure", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns a list of searched expenditure")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<ActionResult<List<EditExpenditureResponseDto>>> SearchExpenditure([FromQuery] SearchRequestDto searchRequest)
        {
            var response = await _expenditureService.SearchExpenditureAsync(searchRequest);
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
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _expenditureService.EditExpenditureAsync(modelRequest);
            return Ok(new { message = response.Item1, expenditure = response.Item2 });
        }


        [HttpDelete]       
        [SwaggerOperation(Summary = "Delete an expenditure", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Delete an expenditure sucessfully")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> DeleteExpenditureAsync(int expenditureId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _expenditureService.DeleteExpenditureAsync(expenditureId);
            return Ok(response);
        }
    }
}
