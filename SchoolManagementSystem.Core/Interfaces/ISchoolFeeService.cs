
using SchoolManagementSystem.Core.DTOs.Requests;


namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ISchoolFeeService
    {
        Task<SchoolFeeDto> SetSchoolFee(ClassFeeDto classFee);
        Task<List<SchoolFeeDto>> ViewSchoolFees();
        Task<bool> EditSchoolFee(int feeId, ClassFeeDto updatedFee);
    }
}
