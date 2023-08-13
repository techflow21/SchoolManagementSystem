namespace SchoolManagementSystem.Core.Entities
{
    public class TeacherSubject : EntityBase
    {
      
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}