using System;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ITeachingStaff
    {

        Task<TeacherModel> AddingTeachingStaff(TeachingStaffModel teachingStaff);

        Task<TeacherModel> UpdateTeachingStaff(string TeacherID, TeachingStaffModel teachingStaff);

        Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubject(Subject Subject);

        Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificClass(Class Class);

        Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubjectAndClass(ClassAndSubjectModel classAndSubjectModel);

        Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel);

        Task<IEnumerable<TeacherWithSubjectAndClassModel>> GetAllTeachingStaffWithClassAndSubjectOnly();

        Task<IEnumerable<TeacherModel>> GetAllTeachingStaff();

        Task<IEnumerable<Subject>> GetAllSubjectOfTeacherByTeacherID(string TeacherID);

        Task<IEnumerable<Class>> GetAllClassOfTeacherByTeacherID(string TeacherID);
        
        Task<TeacherModel> GetTeachingStaffByTeacherID(string TeacherID);

        Task<bool> DeleteTeachingByID(string TeacherID);

        Task<TeacherWithSubjectAndClassModel> AssignSubjectByTeacherID(AddDataModel addSubjectModel); 

        Task<TeacherWithSubjectAndClassModel> AssignClassByTeacherID(AddDataModel addClass);


    }
}

