using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.DTOs.Requests;

public class AddExpenseDto
{
    public ExpenseType? Type { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }
}
