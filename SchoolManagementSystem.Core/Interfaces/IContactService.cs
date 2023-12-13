using SchoolManagementSystem.Core.DTOs.Requests;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IContactService
    {
        Task SubmitContactForm(ContactRequestDto request);
    }
}
