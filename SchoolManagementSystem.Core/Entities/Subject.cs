﻿namespace SchoolManagementSystem.Core.Entities
{
    public class Subject : EntityBase
    {
        public string Name { get; set; }

        public ICollection<TeacherSubject> TeacherSubject { get; set; }

    }
}
