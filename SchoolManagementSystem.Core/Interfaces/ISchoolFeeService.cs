
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ISchoolFeeService
    {
        Task AddSchoolFee(SchoolFeeDto schoolFeeDto);
        Task<string> UpdateSchoolFee(int id, SchoolFeeDto schoolFeeDto);
        Task<List<SchoolFeeResponse>> GetAllSchoolFees();
        Task DeleteSchoolFee(int id);
        Task<decimal> GetTotalSchoolFees();
        Task<decimal> GetTotalSchoolFeesOfClass(string className);
        Task<List<SchoolFeeResponse>> GetAllSchoolFeesOfClass(string className);
    }
}
