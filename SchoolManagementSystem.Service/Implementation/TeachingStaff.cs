﻿using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class TeachingStaff : ITeachingStaff
    {
        private readonly IRepository<Teacher> _teacher;

        private readonly IUnitOfWork _unitOfWork;


       public TeachingStaff(IUnitOfWork unitOfWork)
        {
            _teacher = unitOfWork.GetRepository<Teacher>();
            _unitOfWork = unitOfWork;

        }

        public async Task<TeacherModel> AddingTeachingStaff(TeachingStaffModel teachingStaffModel)
        {
            var teacher = MapModelToTeacher(teachingStaffModel);

            teacher.DateRegistered = DateTime.Now;

            teacher.TeacherID = $"{teacher.DateOfBirth}/randonNum/{teacher.DateRegistered}/randomNum";

            await _teacher.AddAsync(teacher);
            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToModel(teacher);
        }

        public async Task<TeacherWithSubjectAndClassModel> AssignClassByTeacherID(AddDataModel addClass)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == addClass.TeacherID);

            if (teacher == null)
            {
                throw new FileNotFoundException($"TeacherID {addClass.TeacherID} not found in database");
            }

            teacher.Classes.Add(addClass.addClass);

            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToSubjectAndClassModel(teacher);


        }

        public async Task<TeacherWithSubjectAndClassModel> AssignSubjectByTeacherID(AddDataModel addSubjectModel)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == addSubjectModel.TeacherID);

            if (teacher == null)
            {
                throw new FileNotFoundException($"TeacherID {addSubjectModel.TeacherID} not found in database");
            }

            teacher.Subjects.Add(addSubjectModel.subject);

            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToSubjectAndClassModel(teacher);
        }

        public async Task<bool> DeleteTeachingByID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new FileNotFoundException($"TeacherID {TeacherID} not found in database");
            }

            await _teacher.DeleteAsync(teacher);

            var result = await _unitOfWork.SaveChangesAsync();

            if (result == 0) 
	        {
                return false;
	        }
            return true;
        }

        public async Task<IEnumerable<Class>> GetAllClassOfTeacherByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new FileNotFoundException($"TeacherID {TeacherID} not found in database");
            }

            return teacher.Classes;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectOfTeacherByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new FileNotFoundException($"TeacherID {TeacherID} not found in database");
            }

            return teacher.Subjects;
        }

        public async Task<IEnumerable<TeacherModel>> GetAllTeachingStaff()
        {
            var teachers = await _teacher.GetAllAsync();

            return teachers.Select(MapTeacherToModel);
        }

        public async Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificClass(Class Class)
        {
            var teachers = await _teacher.GetByAsync(t => t.Classes.Contains(Class));

            if (teachers == null)
            {
                throw new FileNotFoundException($"No Teacher with Class {Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        public async Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubject(Subject Subject)
        {
            var teachers = await _teacher.GetByAsync(t => t.Subjects.Contains(Subject));

            if (teachers == null)
            {
                throw new FileNotFoundException($"No Teacher with Subject {Subject} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        public async Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubjectAndClass(ClassAndSubjectModel classAndSubjectModel)
        {
            var teachers = await _teacher.GetByAsync(t => t.Classes.Contains(classAndSubjectModel.Class) && t.Subjects.Contains(classAndSubjectModel.Subject));

            if (teachers == null)
            {
                throw new FileNotFoundException($"No Teacher with Subject {classAndSubjectModel.Subject} and Class {classAndSubjectModel.Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        public async Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel)
        {
            var teachers = await _teacher.GetByAsync(t => t.Classes.Contains(classAndSubjectModel.Class) || t.Subjects.Contains(classAndSubjectModel.Subject));

            if (teachers == null)
            {
                throw new FileNotFoundException($"No Teacher with Subject {classAndSubjectModel.Subject} and Class {classAndSubjectModel.Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        public async Task<IEnumerable<TeacherWithSubjectAndClassModel>> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _teacher.GetAllAsync();

            return teachers.Select(MapTeacherToSubjectAndClassModel);
        }

        public async Task<TeacherModel> GetTeachingStaffByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            return MapTeacherToModel(teacher);
        }

        public async Task<TeacherModel> UpdateTeachingStaff(string TeacherID, TeachingStaffModel teachingStaff)
        {
            var existingTeacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (existingTeacher == null)
            {
                throw new FileNotFoundException($"TeacherID {TeacherID} not found in database"); 
	        }

            MapModelToTeacher(teachingStaff);

            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToModel(existingTeacher);
        }

        private Teacher MapModelToTeacher(TeachingStaffModel teachingStaffModel , Teacher existingTeacher = null) 
	    {
            if (existingTeacher == null)
            {
                existingTeacher = new Teacher();
            }

            existingTeacher.FirstName = teachingStaffModel.FirstName;
            existingTeacher.LastName = teachingStaffModel.LastName;
            existingTeacher.MiddleName = teachingStaffModel.MiddleName;
            existingTeacher.Address = teachingStaffModel.Address;
            existingTeacher.LGA = teachingStaffModel.LGA;
            existingTeacher.StateOfOrigin = teachingStaffModel.StateOfOrigin;
            existingTeacher.Email = teachingStaffModel.Email;
            existingTeacher.PhoneNumber = teachingStaffModel.PhoneNumber;
            existingTeacher.DateOfBirth = teachingStaffModel.DateOfBirth;
            existingTeacher.ImageUrl = teachingStaffModel.ImageUrl;

            return existingTeacher;
                  
        }

        private TeacherModel MapTeacherToModel(Teacher teacher)
        {
            return new TeacherModel
            {
                id = teacher.Id,
                TeacherID = teacher.TeacherID,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                MiddleName = teacher.MiddleName,
                Address = teacher.Address,
                LGA = teacher.LGA,
                StateOfOrigin = teacher.StateOfOrigin,
                Email = teacher.Email,
                PhoneNumber = teacher.PhoneNumber,
                DateOfBirth = teacher.DateOfBirth,
                ImageUrl = teacher.ImageUrl,
                DateRegistered = teacher.DateRegistered
              };
        
   
	    }

        private TeacherWithSubjectAndClassModel MapTeacherToSubjectAndClassModel(Teacher teacher)
        {
            return new TeacherWithSubjectAndClassModel
            {
                TeacherID = teacher.TeacherID,
		        FirstName = teacher.FirstName,
		        LastName = teacher.LastName,
		        MiddleName = teacher.MiddleName,
		        Classes = teacher.Classes,
		        subjects = teacher.Subjects
		         
	        };

	    }


    }
}

