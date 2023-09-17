using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Tls;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Exceptions;
using SchoolManagementSystem.Core.Interfaces;
using SchoolManagementSystem.Infrastructure.DataContext;
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

        private readonly ApplicationDbContext _context;



        public TeacherClassAndSubjectService(IUnitOfWork unitOfWork, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;

            _teacher = unitOfWork.GetRepository<Teacher>();

            _teacherClass = unitOfWork.GetRepository<TeacherClass>();

            _teacherSubject = unitOfWork.GetRepository<TeacherSubject>();

            _class = unitOfWork.GetRepository<Class>();

            _subject = unitOfWork.GetRepository<Subject>();

            _context = context;


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



            var getTeacher = await _teacher.GetSingleByAsync(t => t.TeacherID == addClass.TeacherID,
                include: t => t.Include(t => t.TeacherClass)
               .ThenInclude(c => c.Class)
               .Include(t => t.TeacherSubject)
               .ThenInclude(s => s.Subject));



            var teachersWithInfo = new TeacherWithSubjectAndClassModel
            {
                TeacherID = getTeacher.TeacherID,
                FirstName = getTeacher.FirstName,
                LastName = getTeacher.LastName,
                MiddleName = getTeacher.MiddleName,
                Classes = getTeacher.TeacherClass.Select(tc => tc.Class).Select(MapGetClassModel).ToList(),
                subjects = getTeacher.TeacherSubject.Select(ts => ts.Subject).Select(MapGetSubjectModel).ToList()
            };
            return teachersWithInfo;


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




            var getTeacher = await _teacher.GetSingleByAsync(t => t.TeacherID == addSubjectModel.TeacherID,
                include: t => t.Include(t => t.TeacherClass)
               .ThenInclude(c => c.Class)
               .Include(t => t.TeacherSubject)
               .ThenInclude(s => s.Subject));
               


            var teachersWithInfo = new TeacherWithSubjectAndClassModel
            {
                TeacherID = getTeacher.TeacherID,
                FirstName = getTeacher.FirstName,
                LastName = getTeacher.LastName,
                MiddleName = getTeacher.MiddleName,
                Classes = getTeacher.TeacherClass.Select(tc => tc.Class).Select(MapGetClassModel).ToList(),
                subjects = getTeacher.TeacherSubject.Select(ts => ts.Subject).Select(MapGetSubjectModel).ToList()
            };
            return teachersWithInfo;
        }


        public async Task<IEnumerable<TeacherWithSubjectAndClassModel>> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _context.Teachers
                .Include(t => t.TeacherClass)
                .ThenInclude(c => c.Class)
                .Include(t => t.TeacherSubject)
                .ThenInclude(s => s.Subject)
                .ToListAsync();


            var teachersWithInfo = teachers.Select(t => new TeacherWithSubjectAndClassModel
            {
                TeacherID = t.TeacherID,
                FirstName = t.FirstName,
                LastName = t.LastName,
                MiddleName = t.MiddleName,
                Classes = t.TeacherClass.Select(tc => tc.Class).Select(MapGetClassModel).ToList(),
                subjects = t.TeacherSubject.Select(ts => ts.Subject).Select(MapGetSubjectModel).ToList()
            }).ToList();




            return teachersWithInfo;
        }


        public async Task<IEnumerable<GetClassModel>> GetAllClassOfTeacherByTeacherID(string TeacherID)
        {
            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID,
               include: t => t.Include(tc => tc.TeacherClass)
               .ThenInclude(c => c.Class));


            if (teacher != null)
            {
                // Extract the class from the TeacherSubject navigation property
                var classes = teacher.TeacherClass.Select(tc => tc.Class).ToList();
                return classes.Select(MapGetClassModel).ToList();
            }

            // If no teacher with the specified TeacherID is found, return an empty list.
            return Enumerable.Empty<GetClassModel>(); ;
        }

        public async Task<IEnumerable<GetSubjectModel>> GetAllSubjectOfTeacherByTeacherID(string TeacherID)
        {

            var teacher = await _teacher.GetSingleByAsync(t => t.TeacherID == TeacherID,
                include: t => t.Include(ts => ts.TeacherSubject)
                .ThenInclude(s => s.Subject));


            if (teacher != null)
            {
                // Extract the subjects from the TeacherSubject navigation property
                var subjects = teacher.TeacherSubject.Select(ts => ts.Subject).ToList();
                return subjects.Select(MapGetSubjectModel).ToList();
            }

            // If no teacher with the specified TeacherID is found, return an empty list.
            return Enumerable.Empty<GetSubjectModel>(); 




        }


        private GetSubjectModel MapGetSubjectModel(Subject subject)
        {

            return new GetSubjectModel()
            {
                id = subject.Id,
                name = subject.Name
            };

        }


        private GetClassModel MapGetClassModel(Class subject)
        {

            return new GetClassModel()
            {
                Id = subject.Id,
                name = subject.Name
            };

        }


        private async Task<TeacherWithSubjectAndClassModel> MapTeacherToSubjectAndClassModelAsync(Teacher teacher)
        {

            

            var teacherSubject = _context.TeacherSubjects.Where(ts => ts.TeacherId == teacher.Id);



            var SubjectList = teacherSubject.Select(teacherSubject => teacherSubject.SubjectId).ToList();



            var getSubjectTasks = SubjectList.Select(subjectId => _subject.GetSingleByAsync(s => s.Id == subjectId));

            IEnumerable<Subject> subjectsResults = (await Task.WhenAll(getSubjectTasks));
            return new TeacherWithSubjectAndClassModel
            {
                TeacherID = teacher.TeacherID,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                MiddleName = teacher.MiddleName,
                //Classes = (IEnumerable<Class>)classList,
                //subjects = subjectsResults.ToList()

            };

            
        }


    }
}

//var transactions = await _transaction.GetAllAsync(include: t => (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Transaction, object>)t.Include(u => u.UserId == userId).Select(u => u.Category == category).ToListAsync());

//private async Task<IEnumerable<Class>> GetClasses(IEnumerable<TeacherClass> teacherClasses)
//{
//    //var teacherClasses = await _teacherClass.Where(tc => tc.TeacherId == teacherId);

//    if (teacherClasses == null)
//    {
//        return Enumerable.Empty<Class>();
//    };

//    var classList = teacherClasses.Select(teacherClass => teacherClass.ClassId).ToList();

//    var getClassesTasks = classList.Select(classId => _class.GetSingleByAsync(c => c.Id == classId));

//    var classResults = await Task.WhenAll(getClassesTasks);





//    return classResults.ToList();
//}


