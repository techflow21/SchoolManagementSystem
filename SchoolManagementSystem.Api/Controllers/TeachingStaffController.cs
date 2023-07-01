using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Core.DTOs.Responses;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachingStaffController : Controller
    {
        private readonly ITeachingStaff _teachingStaff;

        public TeachingStaffController(ITeachingStaff teachingStaff)
        {
            _teachingStaff = teachingStaff;
        }

        [HttpPost]
        public async Task<IActionResult> AddingTeachingStaff(TeachingStaffModel teachingStaff) {

            var teacher = await _teachingStaff.AddingTeachingStaff(teachingStaff);

            return CreatedAtAction(nameof(GetTeachingStaffByTeacherID), new { id = teacher.id }, teacher);
        }

        [HttpPut("{teacherID}")]
        public async Task<IActionResult> UpdateTeachingStaff(string TeacherID, TeachingStaffModel teachingStaffModel)
        {
            var updatedteacher = await _teachingStaff.UpdateTeachingStaff(TeacherID, teachingStaffModel);

            return Ok(updatedteacher);

	    }

        [HttpGet("{subject}")]
        public async Task<IActionResult> GetAllTeachingStaffOfSpecificSubject(Subject Subject)
        {
            var teachers = await _teachingStaff.GetAllTeachingStaffOfSpecificSubject(Subject);

            return Ok(teachers);
	    }


        [HttpGet("{class}")]
        public async Task<IActionResult> GetAllTeachingStaffOfSpecificClass(Class Class)
        {
            var teachers = await _teachingStaff.GetAllTeachingStaffOfSpecificClass(Class);

            return Ok(teachers);
        }

        [HttpGet("{classAndSubjectModel}")]
        public async Task<IActionResult> GetAllTeachingStaffOfSpecificSubjectAndClass(ClassAndSubjectModel classAndSubjectModel)
        {
            var teachers = await _teachingStaff.GetAllTeachingStaffOfSpecificSubjectAndClass(classAndSubjectModel);

            return Ok(teachers);
	    }

        [HttpGet("{classAndSubjectModel}")]
        public async Task<IActionResult> GetAllTeachingStaffOfSpecificSubject_Or_Class(ClassAndSubjectModel classAndSubjectModel)
        {
            var teachers = await _teachingStaff.GetAllTeachingStaffOfSpecificSubject_Or_Class(classAndSubjectModel);

            return Ok(teachers);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _teachingStaff.GetAllTeachingStaffWithClassAndSubjectOnly();

            return Ok(teachers);
	    }

        [HttpGet]
        public async Task<IActionResult> GetAllTeachingStaff()
        {
            var teachers = await _teachingStaff.GetAllTeachingStaff();
            return Ok(teachers);
	    }


        [HttpGet("{teacherID}")]
        public async Task<IActionResult> GetAllSubjectOfTeacherByTeacherID(string TeacherID)
        {
            var subjects = await _teachingStaff.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(subjects);
	    }

        [HttpGet("{teacherID}")]
        public async Task<IActionResult> GetAllClassOfTeacherByTeacherID(string TeacherID)
        {
            var classes = await _teachingStaff.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(classes);
        }

        [HttpGet("{teacherID}")]
        public async Task<IActionResult> GetTeachingStaffByTeacherID(string TeacherID)
        {
            var teacher = await _teachingStaff.GetTeachingStaffByTeacherID(TeacherID);

            return Ok(teacher);
	    }

        [HttpDelete("{teacherID}")]
        public async Task<IActionResult> DeleteTeachingByID(string TeacherID)
        {
            await _teachingStaff.DeleteTeachingByID(TeacherID);

            return NoContent();
        }

        [HttpPut("{addSubjectModel}")]
        public async Task<IActionResult> AssignSubjectByTeacherID(AddDataModel addSubjectModel)
        {
            var teacher = await _teachingStaff.AssignSubjectByTeacherID(addSubjectModel);

            return Ok(teacher);
	    }

        [HttpPut("{addClassModel}")]
        public async Task<IActionResult> AssignClassByTeacherID(AddDataModel addClassModel) 
	    {
            var teacher = await _teachingStaff.AssignClassByTeacherID(addClassModel);

            return Ok(teacher);
        }

    }
}
