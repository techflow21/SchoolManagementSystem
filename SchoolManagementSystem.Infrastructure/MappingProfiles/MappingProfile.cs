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

            CreateMap<Student, StudentDto>().ReverseMap();
            CreateMap<Student, StudentResponseDto>();
        }
    }
}
