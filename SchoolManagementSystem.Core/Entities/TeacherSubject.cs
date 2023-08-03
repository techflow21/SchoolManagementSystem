namespace SchoolManagementSystem.Core.Entities
{
    public class TeacherSubject : EntityBase
    {
        public int Id { get; set; }

        public string TeacherId { get; set; }
        public Teacher teacher { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}