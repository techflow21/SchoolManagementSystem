using AutoMapper;
using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class SalaryService : ISalaryService
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Salary> _salaryRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalaryService(IHttpContextAccessor httpContextAccessor, ILoggerManager logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _salaryRepo = _unitOfWork.GetRepository<Salary>();
        }

        public async Task<Salary> AddStaffSalaryAsync(AddStaffSalaryDto addStaffSalaryDto)
        {
            var salary = _mapper.Map<Salary>(addStaffSalaryDto);
            salary.Date = DateTime.Now;

            await _salaryRepo.AddAsync(salary);
            await _unitOfWork.SaveChangesAsync();
            return salary;
        }

        public async Task<List<SalaryHistoryDto>> ViewSalaryHistoryAsync()
        {
            var salary = await _salaryRepo.GetAllAsync();

            var salaryHistoryDtos = _mapper.Map<List<SalaryHistoryDto>>(salary);

            return salaryHistoryDtos;
        }

        public async Task<string> EditStaffSalaryAsync(EditStaffSalaryDto salaryEditDto)
        {
            var salary = await _salaryRepo.GetByIdAsync(salaryEditDto.Id);

            if (salary == null)
            {
                return "Salary not found";
            }

            salary.StaffCategory = salaryEditDto.StaffCategory;
            salary.AmountPaid = salaryEditDto.AmountPaid;
            salary.Date = DateTime.Now;
                
            _salaryRepo.Update(salary);
            await _unitOfWork.SaveChangesAsync();
            return "Salary updated successfully";
        }
    }
}
