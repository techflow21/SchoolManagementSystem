﻿using System;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ITeachingStaff
    {

        Task<TeacherModel> AddingTeachingStaff(TeachingStaffModel teachingStaff);

        Task<TeacherModel> UpdateTeachingStaff(string TeacherID, TeachingStaffModel teachingStaff);




        Task<IEnumerable<TeacherModel>> SortingTeachingStaff(SortingTeachingStaffModel sortingTeachingStaff, string tenancyId);





      // GetAllTeachingStaffOfSpecificSubject(Subject Subject);

     //GetAllTeachingStaffOfSpecificClass(Class Class);

       //GetAllTeachingStaffOfSpecificSubjectAndClass(ClassAndSubjectModel classAndSubjectModel);

       // GetAllTeachingStaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel);

// GetAllTeachingStaff();






        Task<IEnumerable<TeacherWithSubjectAndClassModel>> GetAllTeachingStaffWithClassAndSubjectOnly();

        Task<IEnumerable<Subject>> GetAllSubjectOfTeacherByTeacherID(string TeacherID);

        Task<IEnumerable<Class>> GetAllClassOfTeacherByTeacherID(string TeacherID);

        Task<TeacherModel> GetTeachingStaffByTeacherID(string TeacherID);

        Task<bool> DeleteTeachingByID(string TeacherID);

        Task<TeacherWithSubjectAndClassModel> AssignSubjectByTeacherID(AddDataModel addSubjectModel, Subject subject); 

        Task<TeacherWithSubjectAndClassModel> AssignClassByTeacherID(AddDataModel addClass, Class @class);


    }
}

