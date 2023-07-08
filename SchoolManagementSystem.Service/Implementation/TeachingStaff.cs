﻿using System;
using Microsoft.Data.SqlClient.Server;
using SchoolManagementSystem.Core.DTOs.Requests;
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

            teacher.TeacherID = $"{teacher.DateRegistered.Year}{teacher.DateRegistered.Day}{GenerateRandomNumber()}";

            await _teacher.AddAsync(teacher);
            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToModel(teacher);
        }

        public async Task<TeacherWithSubjectAndClassModel> AssignClassByTeacherID(AddDataModel addClass, Class @class)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == addClass.TeacherID);

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {addClass.TeacherID} not found in database");
            }

            @class.Name = addClass.Data;

            teacher.Classes.Add(@class);

            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToSubjectAndClassModel(teacher);


        }

        public async Task<TeacherWithSubjectAndClassModel> AssignSubjectByTeacherID(AddDataModel addSubjectModel, Subject subject)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == addSubjectModel.TeacherID);

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {addSubjectModel.TeacherID} not found in database");
            }

            subject.Name = addSubjectModel.Data;

            teacher.Subjects.Add(subject);

            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToSubjectAndClassModel(teacher);
        }

        public async Task<bool> DeleteTeachingByID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {TeacherID} not found in database");
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
                throw new ArgumentNullException($"TeacherID {TeacherID} not found in database");
            }

            return teacher.Classes;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectOfTeacherByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {TeacherID} not found in database");
            }

            return teacher.Subjects;
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



        public async Task<IEnumerable<TeacherModel>> SearchFuntion(string searchquery, string tenancyID)
        {
            var @class = new Class
            {
                TenantId = tenancyID,
                Name = searchquery
            };

            var subject = new Subject
            {
                TenantId = tenancyID,
                Name = searchquery
            };
            
            var teachers = await _teacher.GetByAsync(t => t.FirstName == searchquery ||
            t.MiddleName == searchquery ||
            t.LastName == searchquery ||
            t.Classes.Contains(@class) ||
            t.Subjects.Contains(subject) ||
            t.PhoneNumber == searchquery ||
            t.StateOfOrigin == searchquery ||
            t.LGA == searchquery ||
            t.TeacherID == searchquery ||
            t.Email == searchquery ||
            t.Address == searchquery
            );

            if (teachers == null)
            {
                throw new ArgumentNullException("No Result");
            }

            return teachers.Select(MapTeacherToModel);
        }



        public async Task<IEnumerable<TeacherModel>> SortingTeachingStaff(SortingTeachingStaffModel sortingTeachingStaff, string tenancyId)
        {
           
            // GET By Class
            if (!string.IsNullOrWhiteSpace(sortingTeachingStaff.Class) && string.IsNullOrWhiteSpace(sortingTeachingStaff.Subject))
            {
                var @class = new Class
                {
                    TenantId = tenancyId,
                    Name = sortingTeachingStaff.Class
                };
                return await GetAllTeachingStaffOfSpecificClass(@class);
            }

            // GET By Subject
            if (string.IsNullOrWhiteSpace(sortingTeachingStaff.Class) && !string.IsNullOrWhiteSpace(sortingTeachingStaff.Subject))
            {

                var subject = new Subject
                {
                    TenantId = tenancyId,
                    Name = sortingTeachingStaff.Subject
                };
                return await GetAllTeachingStaffOfSpecificSubject(subject);
            }

            // Get by Class or Subject
            if (!string.IsNullOrWhiteSpace(sortingTeachingStaff.Class) && !string.IsNullOrWhiteSpace(sortingTeachingStaff.Subject) && sortingTeachingStaff.strictlyBoth == false)
            {
                var @class = new Class
                {
                    TenantId = tenancyId,
                    Name = sortingTeachingStaff.Class
                };
                var subject = new Subject
                {
                    TenantId = tenancyId,
                    Name = sortingTeachingStaff.Subject
                };

                var classAndSubject = new ClassAndSubjectModel
                {
                    Class = @class,
                    Subject = subject
                    
                };

                return await GetAllTeachingStaffOfSpecificSubject_Or_Class(classAndSubject);
            }

            // Get by Class And Subject
            if (!string.IsNullOrWhiteSpace(sortingTeachingStaff.Class) && !string.IsNullOrWhiteSpace(sortingTeachingStaff.Subject) && sortingTeachingStaff.strictlyBoth == true)
            {
                var @class = new Class
                {
                    TenantId = tenancyId,
                    Name = sortingTeachingStaff.Class
                };
                var subject = new Subject
                {
                    TenantId = tenancyId,
                    Name = sortingTeachingStaff.Subject
                };

                var classAndSubject = new ClassAndSubjectModel
                {
                    Class = @class,
                    Subject = subject

                };

                return await GetAllTeachingStaffOfSpecificSubjectAndClass(classAndSubject);
            }

            return await GetAllTeachingStaff();

        }


        public async Task<TeacherModel> UpdateTeachingStaff(string TeacherID, TeachingStaffModel teachingStaff)
        {
            var existingTeacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (existingTeacher == null)
            {
                throw new ArgumentNullException($"TeacherID {TeacherID} not found in database"); 
	        }

            MapModelToTeacher(teachingStaff, existingTeacher);

            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToModel(existingTeacher);
        }







        private Teacher MapModelToTeacher(TeachingStaffModel teachingStaffModel , Teacher existingTeacher = null) 
	    {
      
            if (existingTeacher == null)
            {
                existingTeacher = new Teacher();
            }

            var dateTime = DateTime.Now;

            if (teachingStaffModel.DateOfBirth == dateTime)
            {

                

                DateTime existingdOB = existingTeacher.DateOfBirth;

                teachingStaffModel.DateOfBirth = existingdOB;
            }

            existingTeacher.FirstName = teachingStaffModel.FirstName ?? existingTeacher.FirstName;
            existingTeacher.LastName = teachingStaffModel.LastName ?? existingTeacher.LastName;
            existingTeacher.MiddleName = teachingStaffModel.MiddleName ?? existingTeacher.MiddleName;
            existingTeacher.Address = teachingStaffModel.Address ?? existingTeacher.Address;
            existingTeacher.LGA = teachingStaffModel.LGA ?? existingTeacher.LGA;
            existingTeacher.StateOfOrigin = teachingStaffModel.StateOfOrigin ?? existingTeacher.StateOfOrigin;
            existingTeacher.Email = teachingStaffModel.Email ?? existingTeacher.Email;
            existingTeacher.PhoneNumber = teachingStaffModel.PhoneNumber ?? existingTeacher.PhoneNumber;
            existingTeacher.DateOfBirth = teachingStaffModel.DateOfBirth;
            existingTeacher.ImageUrl = teachingStaffModel.ImageUrl ?? existingTeacher.ImageUrl;

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
                //ImageUrl = teacher.ImageUrl,
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

        private string GenerateRandomNumber()
        {
            Random random = new Random();

            var randomNumber = random.Next(1000, 9999);


            return randomNumber.ToString();
        }

        private async Task<IEnumerable<TeacherModel>> GetAllTeachingStaff()
        {
            var teachers = await _teacher.GetAllAsync();

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificClass(Class Class)
        {
            var teachers = await _teacher.GetByAsync(t => t.Classes.Contains(Class));

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Class {Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubject(Subject Subject)
        {
            var teachers = await _teacher.GetByAsync(t => t.Subjects.Contains(Subject));

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Subject {Subject} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubjectAndClass(ClassAndSubjectModel classAndSubjectModel)
        {
            var teachers = await _teacher.GetByAsync(t => t.Classes.Contains(classAndSubjectModel.Class) && t.Subjects.Contains(classAndSubjectModel.Subject));

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Subject {classAndSubjectModel.Subject} and Class {classAndSubjectModel.Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<TeacherModel>> GetAllTeachingStaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel)
        {
            var teachers = await _teacher.GetByAsync(t => t.Classes.Contains(classAndSubjectModel.Class) || t.Subjects.Contains(classAndSubjectModel.Subject));

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Subject {classAndSubjectModel.Subject} and Class {classAndSubjectModel.Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

    }
}

