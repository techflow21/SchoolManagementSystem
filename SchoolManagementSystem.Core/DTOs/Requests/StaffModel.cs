using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using SchoolManagementSystem.Core.Entities;
using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Core.Enums;
using System.Globalization;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class StaffModel
    {
        [Required(ErrorMessage = "FirstName is Required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "LastName is Required")]
        public string LastName { get; set; } = null!;

        public string MiddleName { get; set; } = null;

        [Required(ErrorMessage = "Staff Category selection is Required")]
        public StaffCategory StaffCategory { get; set; }

        public string Address { get; set; } = null;

        public string LGA { get; set; } = null;

        public string StateOfOrigin { get; set; } = null;

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(pattern: @"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Email format is wrong")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone Number is Required")]
        [RegularExpression(pattern: @"^\d+$", ErrorMessage = "Numbers only")]
        public string PhoneNumber { get; set; } = null!;

        [Required(ErrorMessage = "Date of Birth is Required eg:8/2/2023 3:21:27 PM")]
        public DateTime DateOfBirth { get; set; }

        public string? Duty { get; set; }// should be made required at frontend

        [Required(ErrorMessage = "Profile Photo is Required")]
        [DisplayName("Profile Photo")]
        public IFormFile? ImageUrl { get; set; } 



    }
}

