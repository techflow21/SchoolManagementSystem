using System;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class ClassAndSubjectModel
    {
        public Class Class { get; set; }

        public Subject Subject { get; set; }
    }
}

