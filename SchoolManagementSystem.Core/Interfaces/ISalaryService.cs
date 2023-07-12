using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ISalaryService
    {
        Task<Salary> AddStaffSalaryAsync(AddStaffSalaryDto addStaffSalaryDto);
        Task<List<SalaryHistoryDto>> ViewSalaryHistoryAsync();
        Task<string> EditStaffSalaryAsync(EditStaffSalaryDto salaryEditDto);
    }
}
