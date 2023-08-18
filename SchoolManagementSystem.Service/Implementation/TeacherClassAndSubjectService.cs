using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Exceptions;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Infrastructure.Repository;
using SchoolManagementSystem.Service.ExternalServices;

namespace SchoolManagementSystem.Service.Implementation
{
    public class TeacherClassAndSubjectService : ITeacherClassAndSubject
    {
        private readonly IRepository<Teacher> _teacher;


        private readonly IRepository<Class> _class;

        private readonly IRepository<Subject> _subject;

        private readonly IRepository<TeacherClass> _teacherClass;

        private readonly IRepository<TeacherSubject> _teacherSubject;

        private readonly IUnitOfWork _unitOfWork;

        public TeacherClassAndSubjectService(IUnitOfWork unitOfWork)
        {
            _teacher = unitOfWork.GetRepository<Teacher>();

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
                TeacherId = teacher.Id,

                ClassId = checkClass.Id

            };

            var existingTeacherClass = await _teacherClass.GetSingleByAsync(tc => tc.TeacherId == teacherClass.TeacherId && tc.ClassId == teacherClass.ClassId);

            if (existingTeacherClass != null)
            {
                throw new DuplicateEntryException($"Teacher have already been assigned to this class");        
            }

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
                TeacherId = teacher.Id,

                SubjectId = subject.Id
            };
            var existingTeacherSubject = await _teacherSubject.GetSingleByAsync(ts => ts.TeacherId == teacherSubject.TeacherId && ts.SubjectId == teacherSubject.SubjectId);

            if (existingTeacherSubject != null)
            {
                throw new DuplicateEntryException($"Teacher have already been assigned to this Subject");

                
            }

            await _teacherSubject.AddAsync(teacherSubject);

            await _unitOfWork.SaveChangesAsync();

            return await MapTeacherToSubjectAndClassModelAsync(teacher);
        }


        public async Task<IEnumerable<TeacherWithSubjectAndClassModel>> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _teacher.GetAllAsync();

            

            IEnumerable<Task<TeacherWithSubjectAndClassModel>> tasks =  teachers.Select(async teacher => await MapTeacherToSubjectAndClassModelAsync(teacher));

            //Task<TeacherWithSubjectAndClassModel>[] taskArray = tasks.ToArray();

            IEnumerable<TeacherWithSubjectAndClassModel> results = await Task.WhenAll(tasks);

            Task.WaitAll();

            return results;
        }


        public async Task<IEnumerable<Class>> GetAllClassOfTeacherByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {TeacherID} not found in database");
            }

            var teacherModel = await MapTeacherToSubjectAndClassModelAsync(teacher);

            return teacherModel.Classes;
        }

        public async Task<IEnumerable<Subject>> GetAllSubjectOfTeacherByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID);

            if (teacher == null)
            {
                throw new ArgumentNullException($"TeacherID {TeacherID} not found in database");
            }

            var teacherModel = await MapTeacherToSubjectAndClassModelAsync(teacher);

            return teacherModel.subjects;
        }





        private async Task<TeacherWithSubjectAndClassModel> MapTeacherToSubjectAndClassModelAsync(Teacher teacher)
        {
            var teacherClasses = await _teacherClass.Where(tc => tc.TeacherId == teacher.Id);

            //if (teacherClasses == null)
            //{
            //    //return Enumerable.Empty<Class>();
            //};

            var classList = teacherClasses.Select(teacherClass => teacherClass.ClassId).ToList();

            var getClassesTasks = classList.Select(classId => _class.GetSingleByAsync(c => c.Id == classId));
            //var classResults = await Task.WhenAll(getClassesTasks);

            var teacherSubject = await _teacherSubject.Where(ts => ts.TeacherId == teacher.Id);

            //if (teacherSubject == null)
            //{
            //    //return Enumerable.Empty<Subject>();
            //};

            var SubjectList = teacherSubject.Select(teacherSubject => teacherSubject.SubjectId).ToList();



            var getSubjectTasks = SubjectList.Select(subjectId => _subject.GetSingleByAsync(s => s.Id == subjectId));

            (IEnumerable<Subject> subjectsResults, IEnumerable<Class> classResults) = ( await Task.WhenAll(getSubjectTasks), await Task.WhenAll(getClassesTasks));

            Task.WaitAll();

            return new TeacherWithSubjectAndClassModel
            {
                TeacherID = teacher.TeacherID,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                MiddleName = teacher.MiddleName,
                Classes  = classResults.ToList(),
                subjects = subjectsResults.ToList()

            };

        }

        //private async Task<IEnumerable<Class>> GetClasses(string teacherId)
        //{
        //    var teacherClasses = await _teacherClass.Where(tc => tc.TeacherId == teacherId);

        //    var @classes = teacherClasses.Select(teacherClass => teacherClass.Class).ToList();

        //    return @classes;

        //}

        //private async Task<IEnumerable<Class>> GetClasses(IEnumerable<TeacherClass> teacherClasses)
        //{
        //    //var teacherClasses = await _teacherClass.Where(tc => tc.TeacherId == teacherId);

        //    if (teacherClasses == null)
        //    {
        //        return Enumerable.Empty<Class>();
        //    };

        //    var classList =  teacherClasses.Select(teacherClass => teacherClass.ClassId).ToList();

        //    var getClassesTasks =  classList.Select(classId => _class.GetSingleByAsync(c => c.Id == classId));
        //    var classResults = await Task.WhenAll(getClassesTasks);

        //    Task.WaitAll();

            

        //    return classResults.ToList();
        //}


        //private async Task<IEnumerable<Subject>> GetSubjects(int teacherId)
        //{

        //    var teacherSubject = await _teacherSubject.Where(ts => ts.TeacherId == teacherId);

        //    if (teacherSubject == null)
        //    {
        //        return  Enumerable.Empty <Subject>();
        //    };

        //    var SubjectList = teacherSubject.Select(teacherSubject => teacherSubject.SubjectId).ToList();

          

        //    var getSubjectTasks = SubjectList.Select(subjectId => _subject.GetSingleByAsync(s => s.Id == subjectId));
        //    var subjectsResults = await Task.WhenAll(getSubjectTasks);
        //    Task.WaitAll();

        //    return subjectsResults.ToList();

        //}

        
    }
}

//var transactions = await _transaction.GetAllAsync(include: t => (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Transaction, object>)t.Include(u => u.UserId == userId).Select(u => u.Category == category).ToListAsync());
