using System;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface IStaff
    {

        Task<StaffResponseModel> AddingStaff(StaffModel teachingStaff);

        Task<StaffResponseModel> UpdateTeachingStaff(SelectStaffModel selectStaffModel, StaffModel teachingStaff);

        Task<IEnumerable<StaffResponseModel>> SortingTeachingStaff(SortingTeachingStaffModel sortingTeachingStaff);     

        Task<IEnumerable<StaffResponseModel>> GetAllNonTeachingStaff();

        Task<StaffResponseModel> GetStaffByStaffID(SelectStaffModel selectStaffModel);

        Task<IEnumerable<StaffResponseModel>> SearchFuntion(string searchquery); 

        Task<bool> DeleteStaffByID(SelectStaffModel selectStaffModel);

    }
}

// GetAllTeachingStaffOfSpecificSubject(Subject Subject);

//GetAllTeachingStaffOfSpecificClass(Class Class);

//GetAllTeachingStaffOfSpecificSubjectAndClass(ClassAndSubjectModel classAndSubjectModel);

// GetAllTeachingStaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel);

// GetAllTeachingStaff();




