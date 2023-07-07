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

    [HttpPut("UpdateIncomeAsync")]
    public async Task<IActionResult> UpdateIncomeAsync(UpdateExpenseDto updateExpenseDto)
    {
        var expense = await _manageIncome.UpdateIncomeAsync(updateExpenseDto);

        if (expense is null) return BadRequest();

        return Ok(expense);
    }

    [HttpDelete("DeleteIncomeAsync")]
    public async Task<IActionResult> DeleteIncomeAsync(int id)
    {
        var expense = await _manageIncome.DeleteIncomeAsync(id);

        if (expense is null) return BadRequest();

        return Ok(expense);
    }

    [HttpGet("GetAllIncomeAsync")]
    public async Task<IActionResult> GetAllIncomeAsync()
    {
        var expenses = await _manageIncome.GetAllIncomeAsync();

        if (expenses is null) return BadRequest();

        return Ok(expenses);
    }

    [HttpGet("GetIncomeByIdAsync")]
    public async Task<IActionResult> GetIncomeByIdAsync(int id)
    {
        var expense = await _manageIncome.GetIncomeByIdAsync(id);

        if (expense is null) return BadRequest();

        return Ok(expense);
    }
}
