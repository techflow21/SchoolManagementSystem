using SchoolManagementSystem.Core.Contracts;
using SchoolManagementSystem.Core.DTOs.Requests;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ISubjectService
    {
        Task<ServiceResponse<string>> AddSubjectAsync(SubjectDto addSubject);
        Task<ServiceResponse<string>> UpdateSubjectAsync(int Id, SubjectDto updateSubject);
        Task<List<SubjectDto>> ViewAllSubjectsAsync();
        Task<ServiceResponse<string>> DeleteSubjectAsync(int Id);
    }
}
