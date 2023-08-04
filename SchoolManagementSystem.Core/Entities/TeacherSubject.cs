namespace SchoolManagementSystem.Core.Entities
{
    public class TeacherSubject : EntityBase
    {
       

        public string TeacherID { get; set; }
        public Teacher Teachers { get; set; }

        public int SubjectId { get; set; }
        public Subject Subjects { get; set; }
    }
}