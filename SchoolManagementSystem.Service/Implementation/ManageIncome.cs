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
            Description = addExpenseDto.Description,
            Amount = addExpenseDto.Amount
        };

        await _expenseRepository.AddAsync(newExpense);

        status.StatusCode = 1;
        status.Message = "Income added successfully";
        return status;
    }

    public async Task<Status> UpdateIncomeAsync(UpdateExpenseDto updateExpenseDto)
    {
        var status = new Status();

        var expense = await _expenseRepository.GetByIdAsync(updateExpenseDto.Id);

        if (expense is null)
        {
            status.StatusCode = 0;
            status.Message = "Income not found";
            return status;
        }

        expense.Type = updateExpenseDto.Type;
        expense.Description = updateExpenseDto.Description;
        expense.Amount = updateExpenseDto.Amount;

        await _expenseRepository.UpdateAsync(expense);

        status.StatusCode = 1;
        status.Message = "Income updated successfully";
        return status;
    }

    public async Task<Status> DeleteIncomeAsync(int id)
    {
        var status = new Status();

        var expense = await _expenseRepository.GetByIdAsync(id);


        if (expense is null)
        {
            status.StatusCode = 0;
            status.Message = "Income not found";
            return status;
        }

        await _expenseRepository.DeleteAsync(expense);

        status.StatusCode = 1;
        status.Message = "Income deleted successfully";
        return status;
    }

    public async Task<IEnumerable<Expense>> GetAllIncomeAsync()
    {
        var expenses = await _expenseRepository.GetAllAsync();

        if (expenses is null) throw new Exception("Not found!");

        return expenses;
    }


    public async Task<AddExpenseDto> GetIncomeByIdAsync(int id)
    {
        var expense = await _expenseRepository.GetByIdAsync(id);

        if (expense is null)
        {
            throw new Exception("Not found!");
        }

        return new AddExpenseDto
        {
            Type = expense.Type,
            Description = expense.Description,
            Amount = expense.Amount
        };
    }
}
