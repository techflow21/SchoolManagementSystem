using System;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class AddDataModel
    {
        public string TeacherID { get; set; } = null!;

        public int DataID { get; set; } 
    }
}

