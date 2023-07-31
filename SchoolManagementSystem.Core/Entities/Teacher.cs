namespace SchoolManagementSystem.Core.Entities
{
    public class Teacher : EntityBase
    {
        public string TeacherID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Address { get; set; }
        public string? LGA { get; set; }
        public string? StateOfOrigin { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ICollection<Subject> Subjects { get; set; } = null;
        public ICollection<Class> Classes { get; set; } = null;
        public string? ImageUrl { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
