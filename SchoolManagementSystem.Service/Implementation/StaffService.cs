using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Enums;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Service.Implementation
{
    public class StaffService : IStaff
    {
        private readonly IRepository<Teacher> _teacher;

        private readonly IRepository<NonTeacher> _nonTeacher;

        private readonly IRepository<Class> _class;

        private readonly IRepository<Subject> _subject;

        private readonly IRepository<TeacherClass> _teacherClass;

        private readonly IRepository<TeacherSubject> _teacherSubject;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IPhotoUploadService _photoUploadService;


        public StaffService(IUnitOfWork unitOfWork, IPhotoUploadService photoUploadService)
        {
            _teacher = unitOfWork.GetRepository<Teacher>();

            _nonTeacher = unitOfWork.GetRepository<NonTeacher>();

            _teacherClass = unitOfWork.GetRepository<TeacherClass>();

            _teacherSubject = unitOfWork.GetRepository<TeacherSubject>();

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



        public async Task<IEnumerable<StaffResponseModel>> SearchFuntion(string searchquery)
        {
           
            var teachers = await _teacher.GetByAsync(
                t =>
                t.TeacherID.Contains(searchquery) ||
                t.FirstName.Contains(searchquery) ||
                t.MiddleName.Contains(searchquery) ||
                t.LastName.Contains(searchquery) ||
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



        public async Task<IEnumerable<StaffResponseModel>> SortingTeachingStaff(SortingTeachingStaffModel sortingTeachingStaff)
        {
           
            // GET By Class
            if (!string.IsNullOrWhiteSpace(sortingTeachingStaff.Class) && string.IsNullOrWhiteSpace(sortingTeachingStaff.Subject))
            {
           
                return await GetAllTeachingStaffOfSpecificClass(sortingTeachingStaff.Class);
            }

            // GET By Subject
            if (string.IsNullOrWhiteSpace(sortingTeachingStaff.Class) && !string.IsNullOrWhiteSpace(sortingTeachingStaff.Subject))
            {

                return await GetAllTeachingStaffOfSpecificSubject(sortingTeachingStaff.Subject);
            }

            // Get by Class or Subject
            if (!string.IsNullOrWhiteSpace(sortingTeachingStaff.Class) && !string.IsNullOrWhiteSpace(sortingTeachingStaff.Subject) && sortingTeachingStaff.strictlyBoth == false)
            {

                var classAndSubject = new ClassAndSubjectModel
                {
                    Class = sortingTeachingStaff.Class,
                    Subject = sortingTeachingStaff.Subject

                };

                return await GetAllTeachingStaffOfSpecificSubject_Or_Class(classAndSubject);
            }

            // Get by Class And Subject
            if (!string.IsNullOrWhiteSpace(sortingTeachingStaff.Class) && !string.IsNullOrWhiteSpace(sortingTeachingStaff.Subject) && sortingTeachingStaff.strictlyBoth == true)
            {
                var classAndSubject = new ClassAndSubjectModel
                {
                    Class = sortingTeachingStaff.Class,
                    Subject = sortingTeachingStaff.Subject

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


        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificClass(string Class)
        {
            var @class = await _class.GetSingleByAsync(c => c.Name.Contains(Class));
         
            if (@class == null)
            {
                throw new ArgumentNullException($"No Class with name {Class} was found in database");
            }

            var teacherClass = await _teacherClass.GetByAsync(tc => tc.ClassId == @class.Id);

            if (teacherClass == null)
            {
                throw new ArgumentNullException($"No Teacher with Class {Class} was found in database");
            }

            var teacherList = await GetTeachersFromTeacherClass(teacherClass);


            return teacherList.Select(MapTeacherToModel);

        }

        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificSubject(string Subject)
        {
            var subject = await _subject.GetSingleByAsync(c => c.Name.Contains(Subject));

            if (subject == null)
            {
                throw new ArgumentNullException($"No Subject with name {Subject} was found in database");
            }

            var teacherSubject = await _teacherSubject.GetByAsync(ts => ts.SubjectId == subject.Id);

            if (teacherSubject == null)
            {
                throw new ArgumentNullException($"No Teacher with Subject {Subject} was found in database");
            }

            var teacherList = await GetTeachersFromTeacherSubject(teacherSubject);


            return teacherList.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificSubjectAndClass(ClassAndSubjectModel classAndSubjectModel)
        {
            var @class = await _class.GetSingleByAsync(c => c.Name.Contains(classAndSubjectModel.Class));
            var teacherClass = await _teacherClass.GetByAsync(tc => tc.ClassId == @class.Id);
            var subject = await _class.GetSingleByAsync(c => c.Name.Contains(classAndSubjectModel.Subject));
            var teacherSubject = await _teacherSubject.GetByAsync(ts => ts.SubjectId == subject.Id);



            var teacherListFromSubject = await GetTeachersFromTeacherSubject(teacherSubject);

            var teacherListFromClass = await GetTeachersFromTeacherClass(teacherClass);

            var teachers = teacherListFromClass.Intersect(teacherListFromSubject);

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Subject {classAndSubjectModel.Subject} or Class {classAndSubjectModel.Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }

        private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel)
        {
            var @class = await _class.GetSingleByAsync(c => c.Name.Contains(classAndSubjectModel.Class));
            var teacherClass = await _teacherClass.GetByAsync(tc => tc.ClassId == @class.Id);
            var subject = await _class.GetSingleByAsync(c => c.Name.Contains(classAndSubjectModel.Subject));
            var teacherSubject = await _teacherSubject.GetByAsync(ts => ts.SubjectId == subject.Id);

           

            var teacherListFromSubject = await GetTeachersFromTeacherSubject(teacherSubject);

            var teacherListFromClass = await GetTeachersFromTeacherClass(teacherClass);

            var teachers = teacherListFromClass.Concat(teacherListFromSubject);

            if (teachers == null)
            {
                throw new ArgumentNullException($"No Teacher with Subject {classAndSubjectModel.Subject} or Class {classAndSubjectModel.Class} was not found in database");
            }

            return teachers.Select(MapTeacherToModel);
        }


        private async Task<IEnumerable<Teacher>> GetTeachersFromTeacherClass(IEnumerable<TeacherClass> teacherClass)
        {
            var TeacherList = teacherClass.Select(teacherClass => teacherClass.TeacherId).ToList();



            var getTeacherTasks = TeacherList.Select(teacherId => _teacher.GetSingleByAsync(t => t.TeacherID == teacherId));
            var teacherResults = await Task.WhenAll(getTeacherTasks);

            return teacherResults.ToList();

        }

        private async Task<IEnumerable<Teacher>> GetTeachersFromTeacherSubject(IEnumerable<TeacherSubject> teacherSubject)
        {
            var TeacherList = teacherSubject.Select(teacherSubject => teacherSubject.TeacherId).ToList();



            var getTeacherTasks = TeacherList.Select(teacherId => _teacher.GetSingleByAsync(t => t.TeacherID == teacherId));
            var teacherResults = await Task.WhenAll(getTeacherTasks);

            return teacherResults.ToList();

        }



    }
}


//private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStcaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel)
//{
//    var teachers = await _teacherClass
//        .GetByAsync(tc => tc.Class.Name.Contains(classAndSubjectModel.Class) || tc.Teacher.Subjects.Any(ts => ts.Name.Contains(classAndSubjectModel.Subject)),
//            include: q => q.Include(tc => tc.Teacher).ThenInclude(t => t.Subjects))
//        .ToListAsync();

//    if (teachers == null || !teachers.Any())
//    {
//        throw new ArgumentNullException($"No Teacher with Subject {classAndSubjectModel.Subject} or Class {classAndSubjectModel.Class} was found in the database");
//    }

//    return teachers.Select(MapTeacherToModel);
//}

//private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStjaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel)
//{
//    var teachers = await _teacherClass
//        .GetByAsync(tc => tc.Class.Name.Contains(classAndSubjectModel.Class) ||
//                          _teacherSubject.Any(ts => ts.subject.Name.Contains(classAndSubjectModel.Subject) && ts.TeacherId == tc.TeacherId),
//            include: q => q.Include(tc => tc.Teacher))
//        .ToListAsync();

//    if (teachers == null || !teachers.Any())
//    {
//        throw new ArgumentNullException($"No Teacher with Subject {classAndSubjectModel.Subject} or Class {classAndSubjectModel.Class} was found in the database");
//    }

//    return teachers.Select(MapTeacherToModel);
//}
//private async Task<IEnumerable<StaffResponseModel>> GetAllTeachingStaffOfSpecificSubject_And_Class(ClassAndSubjectModel classAndSubjectModel)
//{
//    var teachers = await _teacherClass
//        .GetByAsync(tc => tc.Class.Name.Contains(classAndSubjectModel.Class) &&
//                          _teacherSubject.Any(ts => ts.Subject.Name.Contains(classAndSubjectModel.Subject) && ts.TeacherId == tc.TeacherId),
//            include: q => q.Include(tc => tc.Teacher))
//        .ToListAsync();

//    if (teachers == null || !teachers.Any())
//    {
//        throw new ArgumentNullException($"No Teacher with both Subject {classAndSubjectModel.Subject} and Class {classAndSubjectModel.Class} was found in the database");
//    }

//    return teachers.Select(MapTeacherToModel);
//}
