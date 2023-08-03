namespace SchoolManagementSystem.Core.Entities
{
    public class Class : EntityBase
    {
        public string Name { get; set; }

        public ICollection<TeacherClass> TeacherClass { get; set; }
    }
}
