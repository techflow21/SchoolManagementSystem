﻿using System;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;

namespace SchoolManagementSystem.Core.Interfaces
{
    public interface ITeacherClassAndSubject
    {
        Task<TeacherWithSubjectAndClassModel> AssignSubjectByTeacherID(AddDataModel addSubjectModel);

        Task<TeacherWithSubjectAndClassModel> AssignClassByTeacherID(AddDataModel addClass);

        Task<IEnumerable<TeacherWithSubjectAndClassModel>> GetAllTeachingStaffWithClassAndSubjectOnly();

        Task<IEnumerable<GetSubjectModel>> GetAllSubjectOfTeacherByTeacherID(string TeacherID);

        Task<IEnumerable<GetClassModel>> GetAllClassOfTeacherByTeacherID(string TeacherID);
    }
}

