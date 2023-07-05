using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation;

public class ManageIncome : IManageIncome
{
    private readonly IRepository<Expense> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ManageIncome(IUnitOfWork unitOfWork, IRepository<Expense> repository)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
    }
}
