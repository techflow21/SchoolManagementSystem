using AutoMapper;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Infrastructure.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Salary, AddStaffSalaryDto>();
            CreateMap<AddStaffSalaryDto, Salary>();
            CreateMap<Salary, SalaryHistoryDto>();
            
            CreateMap<Expense, ExpenditureHistoryDto>();
            CreateMap<EditExpenditureRequestDto, Expense>();
            CreateMap<Expense, EditExpenditureResponseDto>();
            CreateMap<AddExpenditureDto, Expense>();

            CreateMap<SchoolFee, SchoolFeeDto>().ReverseMap();
            CreateMap<SchoolFee, SchoolFeeResponse>().ReverseMap();
            
            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, StudentResponseDto>();
        }
    }
}
