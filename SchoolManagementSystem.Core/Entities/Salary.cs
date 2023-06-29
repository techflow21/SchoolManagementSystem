
using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.Entities
{
    public class Salary : EntityBase
    {
        public StaffCategory StaffCategory { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime Date { get; set; }
    }
}
