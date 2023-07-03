using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ISalaryService
    {
        Task<Salary> AddStaffSalaryAsync(AddStaffSalaryDto addStaffSalaryDto);
        Task<List<SalaryHistoryDto>> ViewSalaryHistoryAsync();

        Task<string> EditStaffSalaryAsync(EditStaffSalaryDto salaryEditDto);

    }
}
