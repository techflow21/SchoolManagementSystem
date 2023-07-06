using SchoolManagementSystem.Core.Enums;

namespace SchoolManagementSystem.Core.DTOs.Responses
{
    public class ExpenditureHistoryDto
    {
        public ExpenseType Type { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
