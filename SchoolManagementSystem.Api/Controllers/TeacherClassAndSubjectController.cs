using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.Interfaces;

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherClassAndSubjectController : ControllerBase
    {
        private readonly ITeacherClassAndSubject _teacherClassAndSubject ;

        private readonly ITenantResolver _tenantResolver;

        public TeacherClassAndSubjectController( ITeacherClassAndSubject teacherClassAndSubject, ITenantResolver tenantResolver) 
        {
            _teacherClassAndSubject = teacherClassAndSubject;

            _tenantResolver = tenantResolver;
        }




        [HttpGet("allTeachingStaffWithClassAndSubjectOnly")]
        public async Task<IActionResult> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _teacherClassAndSubject.GetAllTeachingStaffWithClassAndSubjectOnly();

            return Ok(teachers);
        }




        [HttpGet("allSubjectOfTeacherByTeacherID")]
        public async Task<IActionResult> GetAllSubjectOfTeacherByTeacherID([Required] string TeacherID)
        {
            var subjects = await _teacherClassAndSubject.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(subjects);
        }

        [HttpGet("allClassesOfTeacherByTeacherID")]
        public async Task<IActionResult> GetAllClassOfTeacherByTeacherID([Required] string TeacherID)
        {
            var classes = await _teacherClassAndSubject.GetAllClassOfTeacherByTeacherID(TeacherID);

            return Ok(classes);
        }


        [HttpPost("addSubject")]
        public async Task<IActionResult> AssignSubjectByTeacherID(AddDataModel addSubjectModel)
        {

            var teacher = await _teacherClassAndSubject.AssignSubjectByTeacherID(addSubjectModel);

            return Ok(teacher);
        }

        [HttpPost("addClass")]
        public async Task<IActionResult> AssignClassByTeacherID(AddDataModel addClassModel)
        {

            var teacher = await _teacherClassAndSubject.AssignClassByTeacherID(addClassModel);

            return Ok(teacher);
        }


    }
}

