using System;
using System.Linq;
using Microsoft.Data.SqlClient.Server;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Service.ExternalServices;

namespace SchoolManagementSystem.Service.Implementation
{
    public class StaffService : IStaff
    {
        private readonly IRepository<Teacher> _teacher;

        private readonly IRepository<NonTeacher> _nonTeacher;

        private readonly IRepository<Class> _class;

        private readonly IRepository<Subject> _subject;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IPhotoUploadService _photoUploadService;


        public StaffService(IUnitOfWork unitOfWork, IPhotoUploadService photoUploadService)
        {
            _teacher = unitOfWork.GetRepository<Teacher>();

            _nonTeacher = unitOfWork.GetRepository<NonTeacher>();

            _unitOfWork = unitOfWork;

            _photoUploadService = photoUploadService;

            _class = unitOfWork.GetRepository<Class>();

            _subject = unitOfWork.GetRepository<Subject>();


        }

        public async Task<StaffResponseModel> AddingStaff(StaffModel staffModel)
        {
            
            switch (staffModel.StaffCategory)
            {
                case StaffCategory.Teaching:
                    var teacher = MapModelToTeacher(staffModel);

                    teacher.DateRegistered = DateTime.Now;

                    if (staffModel.ImageUrl != null)
                    {
                        string imagePath = await _photoUploadService.PhotoUpload(staffModel.ImageUrl);

                        teacher.ImageUrl = imagePath;
                    }



                    teacher.TeacherID = $"{teacher.DateRegistered.Year}{teacher.DateRegistered.Day}{GenerateRandomNumber()}";

                    await _teacher.AddAsync(teacher);
                    await _unitOfWork.SaveChangesAsync();

                    return MapTeacherToModel(teacher);

                case StaffCategory.NonTeaching:
                    var nonTeacher = MapModelToNonTeacher(staffModel);

                    nonTeacher.DateRegistered = DateTime.Now;

                    if (staffModel.ImageUrl != null)
                    {
                        string imagePath = await _photoUploadService.PhotoUpload(staffModel.ImageUrl);

                        nonTeacher.ImageUrl = imagePath;
                    }



                    nonTeacher.NonTeacherID = $"{nonTeacher.DateRegistered.Year}{nonTeacher.DateRegistered.Day}{GenerateRandomNumber()}";

                    await _nonTeacher.AddAsync(nonTeacher);
                    await _unitOfWork.SaveChangesAsync();

                    return MapNonTeacherToModel(nonTeacher);

                default:
                    throw new ArgumentNullException($"Invalid Selection");
            }
        }

        public async Task<TeacherWithSubjectAndClassModel> AssignClassByTeacherID(AddDataModel addClass)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == addClass.TeacherID);

            var checkClass = await _class.GetSingleByAsync(c => c.Id == addClass.DataID);

            if (checkClass == null)
            {
                throw new ArgumentNullException($"ClassID {addClass.DataID} was not found in database");
            }

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {addClass.TeacherID} was not found in database");
            }

            

            teacher.Classes.Add(checkClass);

            

            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToSubjectAndClassModel(teacher);


        }

        public async Task<TeacherWithSubjectAndClassModel> AssignSubjectByTeacherID(AddDataModel addSubjectModel)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == addSubjectModel.TeacherID);

            var subject = await _subject.GetSingleByAsync(c => c.Id == addSubjectModel.DataID);

            if (subject == null)
            {
                throw new ArgumentNullException($"ClassID {addSubjectModel.DataID} was not found in database");
            }

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {addSubjectModel.TeacherID} not found in database");
            }

          
            teacher.Subjects.Add(subject);

            await _unitOfWork.SaveChangesAsync();

            return MapTeacherToSubjectAndClassModel(teacher);
        }


        public async Task<bool> DeleteStaffByID(SelectStaffModel selectStaffModel)
        {
            

            switch (selectStaffModel.StaffCategory)
            {
                case StaffCategory.Teaching:
                    var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == selectStaffModel.StaffID);

                    if (teacher == null)
                    {
                        throw new ArgumentNullException($"StaffID {selectStaffModel.StaffID} not found in database");
                    }

                    await _teacher.DeleteAsync(teacher);

                    var result = await _unitOfWork.SaveChangesAsync();

                    if (result == 0)
                    {
                        return false;
                    }
                    return true;

                case StaffCategory.NonTeaching:
                    var nonTeacher = await _nonTeacher.GetSingleByAsync(n => n.NonTeacherID == selectStaffModel.StaffID);

                    if (nonTeacher == null)
                    {
                        throw new ArgumentNullException($"StaffID {selectStaffModel.StaffID} not found in database");
                    }

                    await _nonTeacher.DeleteAsync(nonTeacher);

                    var nonTeacherResult = await _unitOfWork.SaveChangesAsync();

                    if (nonTeacherResult == 0)
                    {
                        return false;
                    }
                    return true;
                default:
                    throw new ArgumentNullException($"Invalid Operation");
                    
            }
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

        public async Task<StaffResponseModel> GetStaffByStaffID(SelectStaffModel selectStaff)
        {
            
            switch (selectStaff.StaffCategory)
            {
                case StaffCategory.Teaching:
                    var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == selectStaff.StaffID);
                    if (teacher == null)
                    {
                        throw new ArgumentNullException($"TeacherID {selectStaff.StaffID} not found in database");
                    }
                    return MapTeacherToModel(teacher);

                case StaffCategory.NonTeaching:

                    var nonTeacher = await _nonTeacher.GetSingleByAsync(n => n.NonTeacherID == selectStaff.StaffID);

                    if (nonTeacher == null)
                    {
                        throw new ArgumentNullException($"Non-TeacherID {selectStaff.StaffID} not found in database");
                    }

                    return MapNonTeacherToModel(nonTeacher);

                default:

                    throw new ArgumentNullException($"Invalid Selection");
                   
            }

        }



        public async Task<IEnumerable<StaffResponseModel>> SearchFuntion(string searchquery, string tenancyID)
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
            
            var teachers = await _teacher.GetByAsync(
                t =>
                t.TeacherID.Contains(searchquery) ||
                t.FirstName.Contains(searchquery) ||
                t.MiddleName.Contains(searchquery) ||
                t.LastName.Contains(searchquery) ||
                t.Classes.Contains(@class) ||
                t.Subjects.Contains(subject) ||
                t.PhoneNumber.Contains(searchquery) ||
                t.StateOfOrigin.Contains(searchquery) ||
                t.LGA.Contains(searchquery) ||
                t.Email.Contains(searchquery) ||
                t.Address.Contains(searchquery)
            );

            var nonTeachers = await _nonTeacher.GetByAsync(
                n =>
                n.NonTeacherID.Contains(searchquery) ||
                n.FirstName.Contains(searchquery) ||
                n.MiddleName.Contains(searchquery) ||
                n.LastName.Contains(searchquery) ||
                n.Duty.Contains(searchquery)||
                n.PhoneNumber.Contains(searchquery) ||
                n.StateOfOrigin.Contains(searchquery) ||
                n.LGA.Contains(searchquery) ||
                n.Email.Contains(searchquery) ||
                n.Address.Contains(searchquery)
            );

            if (teachers == null && nonTeachers == null)
            {
                throw new ArgumentNullException("No Result");
            }

            if (nonTeachers == null && teachers != null)
            {
                return teachers.Select(MapTeacherToModel);
            }
            if (teachers == null && nonTeachers != null)
            {
                return nonTeachers.Select(MapNonTeacherToModel);
            }

            var teacherModel = teachers.Select(MapTeacherToModel);

            var nonTeacherModel = nonTeachers.Select(MapNonTeacherToModel);

            var staff = teacherModel.Concat(nonTeacherModel);

            return staff;

        }



        public async Task<IEnumerable<StaffResponseModel>> SortingTeachingStaff(SortingTeachingStaffModel sortingTeachingStaff, string tenancyId)
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


        public async Task<StaffResponseModel> UpdateTeachingStaff(SelectStaffModel selectStaffModel, StaffModel staffModel)
        {
           
            switch (selectStaffModel.StaffCategory)
            {
                case StaffCategory.Teaching:
                    var existingTeacher = await _teacher.GetSingleByAsync(t => t.TeacherID == selectStaffModel.StaffID);

                    if (existingTeacher == null)
                    {
                        throw new ArgumentNullException($"TeacherID {selectStaffModel.StaffID} not found in database");
                    }

                    MapModelToTeacher(staffModel, existingTeacher);

                    await _unitOfWork.SaveChangesAsync();

                    return MapTeacherToModel(existingTeacher);

                case StaffCategory.NonTeaching:
                    var existingNonTeacher = await _nonTeacher.GetSingleByAsync(n => n.NonTeacherID == selectStaffModel.StaffID);

                    if (existingNonTeacher == null)
                    {
                        throw new ArgumentNullException($"Non-TeacherID {selectStaffModel.StaffID} not found in database");
                    }

                    MapModelToNonTeacher(staffModel, existingNonTeacher);

                    await _unitOfWork.SaveChangesAsync();

                    return MapNonTeacherToModel(existingNonTeacher);

                default:
                    throw new ArgumentNullException($"Invalid Selection");
            }
        }

        public async Task<IEnumerable<StaffResponseModel>> GetAllNonTeachingStaff()
        {
            var nonTeachers = await _nonTeacher.GetAllAsync();

            return nonTeachers.Select(MapNonTeacherToModel);
        }










        private Teacher MapModelToTeacher(StaffModel teachingStaffModel , Teacher existingTeacher = null) 
	    {
      
            if (existingTeacher == null)
            {
                existingTeacher = new Teacher();
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
            //existingTeacher.ImageUrl = teachingStaffModel.ImageUrl ?? existingTeacher.ImageUrl;

            return existingTeacher;
                  
        }

        private NonTeacher MapModelToNonTeacher(StaffModel StaffModel, NonTeacher existingNonTeacher = null)
        {

            if (existingNonTeacher == null)
            {
                existingNonTeacher = new NonTeacher();
            }




            existingNonTeacher.FirstName = StaffModel.FirstName ;
            existingNonTeacher.LastName = StaffModel.LastName ;
            existingNonTeacher.MiddleName = StaffModel.MiddleName ;
            existingNonTeacher.Address = StaffModel.Address ;
            existingNonTeacher.LGA = StaffModel.LGA ;
            existingNonTeacher.StateOfOrigin = StaffModel.StateOfOrigin ;
            existingNonTeacher.Email = StaffModel.Email ;
            existingNonTeacher.PhoneNumber = StaffModel.PhoneNumber ;
            existingNonTeacher.DateOfBirth = StaffModel.DateOfBirth;
            //existingNonTeacher.ImageUrl = StaffModel.ImageUrl ;

            return existingNonTeacher;

        }

        private StaffResponseModel MapTeacherToModel(Teacher teacher)
        {

            return new StaffResponseModel
            {
                id = teacher.Id,
                StaffID = teacher.TeacherID,
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

        private StaffResponseModel MapNonTeacherToModel(NonTeacher nonTeacher)
        {

            return new StaffResponseModel
            {
                id = nonTeacher.Id,
                StaffID = nonTeacher.NonTeacherID,
                FirstName = nonTeacher.FirstName,
                LastName = nonTeacher.LastName,
                MiddleName = nonTeacher.MiddleName,
                Address = nonTeacher.Address,
                LGA = nonTeacher.LGA,
                StateOfOrigin = nonTeacher.StateOfOrigin,
                Email = nonTeacher.Email,
                PhoneNumber = nonTeacher.PhoneNumber,
                DateOfBirth = nonTeacher.DateOfBirth,
                ImageUrl = nonTeacher.ImageUrl,
                DateRegistered = nonTeacher.DateRegistered
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



        private static string GenerateRandomNumber()
        {
            Random random = new Random();

            var randomNumber = random.Next(1000, 9999);


            return randomNumber.ToString();
        }

        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaff()
        {
            var teachers = await _teacher.GetAllAsync();

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificClass(Class Class)
        {
            var teachers = await _teacher.GetByAsync(t => t.Classes.Contains(Class));

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Class {Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificSubject(Subject Subject)
        {
            var teachers = await _teacher.GetByAsync(t => t.Subjects.Contains(Subject));

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Subject {Subject} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificSubjectAndClass(ClassAndSubjectModel classAndSubjectModel)
        {
            var teachers = await _teacher.GetByAsync(t => t.Classes.Contains(classAndSubjectModel.Class) && t.Subjects.Contains(classAndSubjectModel.Subject));

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Subject {classAndSubjectModel.Subject} and Class {classAndSubjectModel.Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel)
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

