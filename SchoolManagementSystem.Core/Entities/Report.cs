using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.Entities
{
    public class Report : EntityBase
    {
        public ReportType Type { get; set; }
        public string Description { get; set; }
        public DateTime DateReported { get; set; }
    }
}
