namespace SchoolManagementSystem.Core.Entities
{
    public class SchoolSetting : EntityBase
    {
        public string SchoolName { get; set; }
        public string? SchoolImg { get; set;}
        public SchoolSession? SchoolSession { get; set; }
    }
}
