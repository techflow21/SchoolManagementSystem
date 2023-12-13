using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class StudentRequestDto
    {
        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Middle Name")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Phone Number is required")]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "State of Origin is required")]
        [DisplayName("State Of Origin")]
        public string StateOfOrigin { get; set; }

        [Required(ErrorMessage = "Local Government Area is required")]
        public string LGA { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [DisplayName("Profile Photo")]
        public IFormFile? StudentPhoto { get; set; }
    }
}
