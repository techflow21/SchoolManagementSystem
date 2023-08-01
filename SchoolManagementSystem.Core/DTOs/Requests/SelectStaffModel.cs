using System;
using System.ComponentModel.DataAnnotations;
using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class SelectStaffModel
    {
        [Required (ErrorMessage = "StaffID is required")]
        public string StaffID { get; set; }

        [Required (ErrorMessage = "Staff Category selection is required")]
        public StaffCategory StaffCategory { get; set; }
    }
}

