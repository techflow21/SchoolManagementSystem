using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagementSystem.Core.Entities
{
    [NotMapped]
    public class StudentAttendance
    {
        public Student Student { get; set; }
        public bool AttendanceStatus { get; set; }
    }
}
