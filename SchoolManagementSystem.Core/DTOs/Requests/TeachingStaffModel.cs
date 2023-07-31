using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using SchoolManagementSystem.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class TeachingStaffModel
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string LGA { get; set; }
        public string StateOfOrigin { get; set; }

        [RegularExpression(pattern: "/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)*$/", ErrorMessage = "Email format is wrong")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        [DisplayName("Profile Photo")]
        public IFormFile? ImageUrl { get; set; }



    }
}

