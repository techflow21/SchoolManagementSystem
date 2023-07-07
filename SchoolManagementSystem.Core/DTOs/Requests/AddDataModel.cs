using System;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class AddDataModel
    {
        public string TeacherID { get; set; } = null!;

        public string Data { get; set; } = null!;
    }
}

