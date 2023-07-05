using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.Entities;

public class Expense : EntityBase
{
    public ExpenseType Type { get; set; }
    public string? Description { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public DateTime? UpdatedDate { get; set; } = DateTime.Now;
}
