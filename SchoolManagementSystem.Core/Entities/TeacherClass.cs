using System;
using System.Runtime.CompilerServices;

namespace SchoolManagementSystem.Core.Entities
{
    
    public class TeacherClass : EntityBase
    {
        public int Id { get; set; }

        public string TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}

