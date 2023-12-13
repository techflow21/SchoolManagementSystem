using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IClassService
    {
        Task<ServiceResponse<string>> AddClassAsync(ClassRequestDto addClass);
        Task<ServiceResponse<string>> UpdateClassAsync(int Id, ClassRequestDto updateClass);
        Task<List<ClassResponseDto>> ViewAllClassesAsync();
        Task<ServiceResponse<string>> DeleteClassAsync(int Id);
    }
}
