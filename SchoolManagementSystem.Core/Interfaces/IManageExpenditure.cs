using SchoolManagementSystem.Core.DTOs.Responses;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IManageExpenditure
    {
        Task<IEnumerable<ExpenditureHistoryDto>> ViewExpenditureHistoryAsync();
    }
}
