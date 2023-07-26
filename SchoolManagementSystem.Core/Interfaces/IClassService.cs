using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IClassService
    {
        Task<ServiceResponse<string>> AddClassAsync(ClassDto addClass);
        Task<ServiceResponse<string>> UpdateClassAsync(int Id, ClassDto updateClass);
        Task<List<ClassDto>> ViewAllClassesAsync();
        Task<ServiceResponse<string>> DeleteClassAsync(int Id);
    }
}
