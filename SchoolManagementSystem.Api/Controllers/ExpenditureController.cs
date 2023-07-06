using Microsoft.AspNetCore.Mvc;
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


        [HttpGet]
        [Route("get-all-expenditures")]
        [SwaggerOperation(Summary = "Get all expenditures", Description = "requires authorization.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Returns the list of all expenditure history", typeof(IEnumerable<ExpenditureHistoryDto>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, "Internal server error.")]
        public async Task<IActionResult> GetAllExpenditureHistoryAsync()
        {
            var response = await _expenditureService.ViewExpenditureHistoryAsync();
            return Ok(response);
        }

    }
}
