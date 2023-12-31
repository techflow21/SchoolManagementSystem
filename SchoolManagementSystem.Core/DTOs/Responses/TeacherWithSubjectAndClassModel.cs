﻿using System;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.DTOs.Responses
{
    public class TeacherWithSubjectAndClassModel
    {
        public string TeacherID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<Subject> subjects { get; set; }

        public IEnumerable<Class> Classes { get; set; }
    }
}

