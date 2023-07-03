using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.Entities
{
    public class Result : EntityBase
    {
        public Class Class { get; set; }
        public Subject Subject { get; set; }
        public List<Student> Students { get; set; }
        public int CAScore { get; set; }
        public int ExamScore { get; set; }
        public int TotalScore { get; set; }
        public GradeLetter GradeLetter { get; set; }
        public GradeName GradeName { get; set;}
    }
}
