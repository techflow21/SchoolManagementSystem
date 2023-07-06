using AutoMapper;
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

        public async Task<IEnumerable<ExpenditureHistoryDto>> ViewExpenditureHistoryAsync()
        {
            var allExpense = await _expenditureRepo.GetAllAsync();

            var expenditureHistory = allExpense.Where(e => e.Type == ExpenseType.Expenditure);

            var expenditures = _mapper.Map<IEnumerable<ExpenditureHistoryDto>>(expenditureHistory);

            return expenditures;
        }


    }
}
