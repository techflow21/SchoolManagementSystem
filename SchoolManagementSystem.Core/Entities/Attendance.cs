using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.Entities
{
    public class Attendance : EntityBase
    {
        public Class Class { get; set; }
        public List<StudentAttendance> StudentAttendances { get; set; }
    }
}
