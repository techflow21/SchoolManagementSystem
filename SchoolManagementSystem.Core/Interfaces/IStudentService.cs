using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IStudentService
    {
        Task<ServiceResponse<string>> AddStudentAsync(StudentDto addStudent);
        Task<ServiceResponse<string>> UpdateStudentAsync(int Id, StudentDto updateStudent);
        Task<StudentResponseDto> ViewStudentAsync(int Id);
        Task<List<StudentResponseDto>> ViewAllStudentsAsync();
        Task<ServiceResponse<string>> DeactivateStudentAsync(int Id);
        
    }
}

