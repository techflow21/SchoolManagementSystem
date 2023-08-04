using System;
using System.Runtime.CompilerServices;

namespace SchoolManagementSystem.Core.Entities
{
    
    public class TeacherClass : EntityBase
    {
      
        public string TeacherId { get; set; }
        public Teacher Teachers { get; set; }

        public int ClassId { get; set; }
        public Class Classes { get; set; }
    }
}

