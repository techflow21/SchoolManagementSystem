using Microsoft.AspNetCore.Http;
using SchoolManagementSystem.Core.Interfaces;
using System.Net.Http.Headers;

namespace SchoolManagementSystem.Service.ExternalServices
{
    public class PhotoUploadService : IPhotoUploadService
    {
        public async Task<string> PhotoUpload(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException("File is missing or empty");

            var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            string imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            Directory.CreateDirectory(imageFolderPath);

            var fullPath = Path.Combine(imageFolderPath, filename);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("Images", filename);
        }
    }
}
