namespace SchoolManagementSystem.Core.Entities
{
    public class Contact : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
