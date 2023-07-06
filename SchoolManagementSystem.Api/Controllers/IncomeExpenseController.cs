using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class IncomeExpenseController : ControllerBase
{
    private readonly IManageIncome _manageIncome;

    public IncomeExpenseController(IManageIncome manageIncome)
    {
        _manageIncome = manageIncome;
    }

    [HttpPost("AddIncome")]
    public async Task<IActionResult> AddIncome(AddExpenseDto addExpenseDto)
    {
        var expense = await _manageIncome.AddIncomeAsync(addExpenseDto);

        if (expense is null) return BadRequest();

        return CreatedAtAction(nameof(AddIncome), expense);
    }
}
