namespace SchoolManagementSystem.Core.Entities
{
    
    public class TeacherClass : EntityBase
    {
      
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int ClassId { get; set; }
        public Class Class { get; set; }
    }
}

