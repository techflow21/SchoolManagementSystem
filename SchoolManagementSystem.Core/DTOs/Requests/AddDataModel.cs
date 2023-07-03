using System;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class AddDataModel
    {
        public string TeacherID { get; set; }

        public Class addClass{ get; set; }

        public Subject subject { get; set; }
    }
}

