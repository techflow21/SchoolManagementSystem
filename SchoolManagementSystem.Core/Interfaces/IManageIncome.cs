using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces;

public interface IManageIncome
{
    Task<Status> AddIncomeAsync(AddExpenseDto addExpenseDto);
    Task<Status> UpdateIncomeAsync(UpdateExpenseDto updateExpenseDto);
    Task<Status> DeleteIncomeAsync(int id);
    Task<IEnumerable<Expense>> GetAllIncomeAsync();
    Task<AddExpenseDto> GetIncomeByIdAsync(int id);
}
