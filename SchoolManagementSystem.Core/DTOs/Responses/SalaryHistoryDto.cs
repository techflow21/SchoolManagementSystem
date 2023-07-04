using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.DTOs.Responses
{
    public class SalaryHistoryDto
    {
        public StaffCategory StaffCategory { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime Date { get; set; }
    }
}
