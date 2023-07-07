using Microsoft.AspNetCore.Http;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IPhotoUploadService
    {
        Task<string> PhotoUpload(IFormFile file);
    }
}
