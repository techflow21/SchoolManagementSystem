using System;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.DTOs.Responses
{
    public class TeacherWithSubjectAndClassModel
    {
        public string TeacherID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<GetSubjectModel> subjects { get; set; }

        public IEnumerable<GetClassModel> Classes { get; set; }
    }
}

