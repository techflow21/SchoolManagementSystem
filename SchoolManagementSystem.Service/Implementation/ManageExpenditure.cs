using AutoMapper;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class ManageExpenditure : IManageExpenditure
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Expense> _expenditureRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;


        public ManageExpenditure(IUnitOfWork unitOfWork, ILoggerManager logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _expenditureRepo = unitOfWork.GetRepository<Expense>();
        }

        public async Task<Expense> AddExpenditureAsync(AddExpenditureDto request)
        {
            var newExpenditure = _mapper.Map<Expense>(request);

            newExpenditure.Date = DateTime.Now;
            newExpenditure.Type = ExpenseType.Expenditure;

            await _expenditureRepo.AddAsync(newExpenditure);
            await _unitOfWork.SaveChangesAsync();
            return newExpenditure;
        }

        public async Task<(string, EditExpenditureResponseDto)> EditExpenditureAsync(EditExpenditureRequestDto request)
        {
            var expenditureExists = await _expenditureRepo.GetByIdAsync(request.Id);

            if (expenditureExists == null)
            {
                throw new Exception("Expense not found");
            }

            var expenditure = _mapper.Map(request, expenditureExists);
            expenditure.Type = ExpenseType.Expenditure;
            expenditure.UpdatedDate = DateTime.Now;

            var updatedExpenditure = await _expenditureRepo.UpdateAsync(expenditure);
            await _unitOfWork.SaveChangesAsync();

            var result = _mapper.Map<EditExpenditureResponseDto>(updatedExpenditure);

            return ("Expenditure updated successfully", result);
        }

        public async Task<IEnumerable<ExpenditureHistoryDto>> ViewExpenditureHistoryAsync()
        {
            var allExpense = await _expenditureRepo.GetAllAsync();

            var expenditureHistory = allExpense.Where(e => e.Type == ExpenseType.Expenditure);

            var expenditures = _mapper.Map<IEnumerable<ExpenditureHistoryDto>>(expenditureHistory);

            return expenditures;
        }
    }
}
