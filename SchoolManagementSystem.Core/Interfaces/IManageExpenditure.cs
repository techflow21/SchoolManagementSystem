using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IManageExpenditure
    {
        Task<Expense> AddExpenditureAsync(AddExpenditureDto request);

        Task<IEnumerable<ExpenditureHistoryDto>> ViewExpenditureHistoryAsync();

        Task<(string, EditExpenditureResponseDto)> EditExpenditureAsync(EditExpenditureRequestDto request);

        Task<string> DeleteExpenditureAsync(int expenditureId);

        Task<EditExpenditureResponseDto> GetExpenditureByIdAsync(int expenditureId);

        Task<List<EditExpenditureResponseDto>> SearchExpenditureAsync(SearchRequestDto searchRequest);
    }
}
