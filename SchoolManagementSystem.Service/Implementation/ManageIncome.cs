using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation;

public class ManageIncome : IManageIncome
{
    private readonly IRepository<Expense> _expenseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ManageIncome(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _expenseRepository = _unitOfWork.GetRepository<Expense>();
    }

    public async Task<Status> AddIncomeAsync(AddExpenseDto addExpenseDto)
    {
        var status = new Status();

        var newExpense = new Expense
        {
            Type = addExpenseDto.Type,
            Description = addExpenseDto.Description
        };

        await _expenseRepository.AddAsync(newExpense);

        status.StatusCode = 1;
        status.Message = "Income added successfully";
        return status;
    }
}
