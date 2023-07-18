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

        public async Task<string> DeleteExpenditureAsync(int expenditureId)
        {
            var expenditureExists = await _expenditureRepo.GetSingleByAsync(e => e.Id == expenditureId);


            if (expenditureExists == null)
            {
                throw new Exception("Expense not found");
            }

            await _expenditureRepo.DeleteAsync(expenditureExists);

            return "Expenditure deleted successfully";
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

        public async Task<EditExpenditureResponseDto> GetExpenditureByIdAsync(int expenditureId)
        {
            var expenditure = await _expenditureRepo.GetSingleByAsync(e => e.Id == expenditureId);

            if (expenditure == null)
            {
                throw new Exception("Expense not found");
            }

            var result = _mapper.Map<EditExpenditureResponseDto>(expenditure);

            return result;
        }

        public async Task<List<EditExpenditureResponseDto>> SearchExpenditureAsync(SearchRequestDto searchRequest)
        {
            var allExpenditure = await _expenditureRepo.GetAllAsync();

            if (!string.IsNullOrEmpty(searchRequest.Search))
            {
                allExpenditure = allExpenditure.Where(p => p.Description.Contains(searchRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var result = _mapper.Map<List<EditExpenditureResponseDto>>(allExpenditure);

            return result;
        }

        public async Task<IEnumerable<ExpenditureHistoryDto>> ViewExpenditureHistoryAsync()
        {
            var allExpense = await _expenditureRepo.GetByAsync(e => e.Type == ExpenseType.Expenditure);

            var expenditures = _mapper.Map<IEnumerable<ExpenditureHistoryDto>>(allExpense);

            return expenditures;
        }


    }
}
