namespace SchoolManagementSystem.Core.Entities
{
    public class SMSMessage : EntityBase
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public List<Student>? Students { get; set; }
        public List<Teacher>? Teachers { get; set; }
    }
}
