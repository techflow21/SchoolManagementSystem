using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class EditStaffSalaryDto
    {
        public int Id { get; set; }
        public StaffCategory StaffCategory { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
