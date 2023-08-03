using System;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Infrastructure.Repository;
using SchoolManagementSystem.Service.ExternalServices;

namespace SchoolManagementSystem.Service.Implementation
{
    public class TeacherClassAndSubjectService : ITeacherClassAndSubject
    {
        private readonly IRepository<Teacher> _teacher;

        private readonly IRepository<NonTeacher> _nonTeacher;

        private readonly IRepository<Class> _class;

        private readonly IRepository<Subject> _subject;

        private readonly IRepository<TeacherClass> _teacherClass;

        private readonly IRepository<TeacherSubject> _teacherSubject;

        private readonly IUnitOfWork _unitOfWork;

        public TeacherClassAndSubjectService(IUnitOfWork unitOfWork)
        {
            _teacher = unitOfWork.GetRepository<Teacher>();

            _nonTeacher = unitOfWork.GetRepository<NonTeacher>();

            _teacherClass = unitOfWork.GetRepository<TeacherClass>();

            _teacherSubject = unitOfWork.GetRepository<TeacherSubject>();

            _unitOfWork = unitOfWork;          

            _class = unitOfWork.GetRepository<Class>();

            _subject = unitOfWork.GetRepository<Subject>();
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

            var teacherClass = new TeacherClass()
            {


                TeacherId = teacher.TeacherID,

                ClassId = checkClass.Id

            };

            await _teacherClass.AddAsync(teacherClass);

            await _unitOfWork.SaveChangesAsync();

            return await MapTeacherToSubjectAndClassModelAsync(teacher);


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

            var teacherSubject = new TeacherSubject()
            {
                TeacherId = teacher.TeacherID,

                SubjectId = subject.Id
            };


            await _teacherSubject.AddAsync(teacherSubject);

            await _unitOfWork.SaveChangesAsync();

            return await MapTeacherToSubjectAndClassModelAsync(teacher);
        }


        public async Task<IEnumerable<TeacherWithSubjectAndClassModel>> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _teacher.GetAllAsync();

            

            IEnumerable<Task<TeacherWithSubjectAndClassModel>> tasks = teachers.Select(async teacher => await MapTeacherToSubjectAndClassModelAsync(teacher));

            Task<TeacherWithSubjectAndClassModel>[] taskArray = tasks.ToArray();

            IEnumerable<TeacherWithSubjectAndClassModel> results = await Task.WhenAll(taskArray);


            return results;
        }


        public async Task<IEnumerable<Class>> GetAllClassOfTeacherByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {TeacherID} not found in database");
            }

            return await GetClasses(TeacherID);
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectOfTeacherByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {TeacherID} not found in database");
            }

            return await GetSubjects(TeacherID);
        }





        private async Task<TeacherWithSubjectAndClassModel> MapTeacherToSubjectAndClassModelAsync(Teacher teacher)
        {
            


            return new TeacherWithSubjectAndClassModel
            {
                TeacherID = teacher.TeacherID,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                MiddleName = teacher.MiddleName,
                Classes  = await GetClasses(teacher.TeacherID),
                subjects = await GetSubjects(teacher.TeacherID)

            };

        }

        //private async Task<IEnumerable<Class>> GetClasses(string teacherId)
        //{
        //    var teacherClasses = await _teacherClass.Where(tc => tc.TeacherId == teacherId);

        //    var @classes = teacherClasses.Select(teacherClass => teacherClass.Class).ToList();

        //    return @classes;

        //}

        private async Task<IEnumerable<Class>> GetClasses(string teacherId)
        {
            var teacherClasses = await _teacherClass.Where(tc => tc.TeacherId == teacherId);

            if (teacherClasses == null)
            {
                return Enumerable.Empty<Class>();
            };

            var classList = teacherClasses.Select(teacherClass => teacherClass.ClassId).ToList();

            var getClassesTasks = classList.Select(classId => _class.GetSingleByAsync(c => c.Id == classId));
            var classResults = await Task.WhenAll(getClassesTasks);

            return classResults.ToList();
        }


        private async Task<IEnumerable<Subject>> GetSubjects(string teacherId)
        {

            var teacherSubject = await _teacherSubject.Where(ts => ts.TeacherId == teacherId);

            if (teacherSubject == null)
            {
                return  Enumerable.Empty <Subject>();
            };

            var SubjectList = teacherSubject.Select(teacherSubject => teacherSubject.SubjectId).ToList();

          

            var getSubjectTasks = SubjectList.Select(subjectId => _subject.GetSingleByAsync(s => s.Id == subjectId));
            var subjectsResults = await Task.WhenAll(getSubjectTasks);

            return subjectsResults.ToList();

        }

        
    }
}

//var transactions = await _transaction.GetAllAsync(include: t => (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Transaction, object>)t.Include(u => u.UserId == userId).Select(u => u.Category == category).ToListAsync());
