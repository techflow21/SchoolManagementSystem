using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;

namespace SchoolManagementSystem.Core.Interfaces;

public interface IManageIncome
{
    Task<Status> AddIncomeAsync(AddExpenseDto addExpenseDto);
}
