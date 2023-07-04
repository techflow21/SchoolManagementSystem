using SchoolManagementSystem.Core.Enums;


namespace SchoolManagementSystem.Core.DTOs.Requests
{
    public class AddStaffSalaryDto
    {
        public StaffCategory StaffCategory { get; set; }
        public decimal AmountPaid { get; set; }
    }
}
