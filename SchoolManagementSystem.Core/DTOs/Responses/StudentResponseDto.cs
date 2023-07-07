using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.DTOs.Responses
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string StateOfOrigin { get; set; }
        public string? LGA { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Class? Class { get; set; }
        public List<Subject>? Subjects { get; set; }
        public string? ImageUrl { get; set; }
    }
}
