using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ISubjectService
    {
        Task<ServiceResponse<string>> AddSubjectAsync(SubjectRequestDto addSubject);
        Task<ServiceResponse<string>> UpdateSubjectAsync(int Id, SubjectRequestDto updateSubject);
        Task<List<SubjectResponseDto>> ViewAllSubjectsAsync();
        Task<ServiceResponse<string>> DeleteSubjectAsync(int Id);
    }
}
