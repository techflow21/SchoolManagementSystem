﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Api.Extensions;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeachingStaffController : ControllerBase
    {
        private readonly ITeachingStaff _teachingStaff;

        private readonly IHttpContextAccessor _contextAccessor;

        public TeachingStaffController(ITeachingStaff teachingStaff, IHttpContextAccessor contextAccessor)
        {
            _teachingStaff = teachingStaff;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("addingTeachingStaff")]
        public async Task<IActionResult> AddingTeachingStaff(TeachingStaffModel teachingStaff)
        {

            var teacher = await _teachingStaff.AddingTeachingStaff(teachingStaff);

            return CreatedAtAction(nameof(GetTeachingStaffByTeacherID), new { teacher.id }, teacher);
        }

        [HttpPut("updateTeachingStaff")]
        public async Task<IActionResult> UpdateTeachingStaff(string TeacherID, TeachingStaffModel teachingStaffModel)
        {
            var updatedteacher = await _teachingStaff.UpdateTeachingStaff(TeacherID, teachingStaffModel);

            return Ok(updatedteacher);

        }

        [HttpGet("SortingTeachingStaff")]
        public async Task<IActionResult> SortingTeachingStaff(SortingTeachingStaffModel sortingTeachingStaff)
        {

            var tenancyId = _contextAccessor.HttpContext.User.GetUserId();

            var teachers = await _teachingStaff.SortingTeachingStaff(sortingTeachingStaff, tenancyId);

            return Ok(teachers);
        }



        [HttpGet("allTeachingStaffWithClassAndSubjectOnly")]
        public async Task<IActionResult> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _teachingStaff.GetAllTeachingStaffWithClassAndSubjectOnly();

            return Ok(teachers);
        }


        [HttpGet("allSubjectOfTeacherByTeacherID")]
        public async Task<IActionResult> GetAllSubjectOfTeacherByTeacherID(string TeacherID)
        {
            var subjects = await _teachingStaff.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(subjects);
        }

        [HttpGet("allClassesOfTeacherByTeacherID")]
        public async Task<IActionResult> GetAllClassOfTeacherByTeacherID(string TeacherID)
        {
            var classes = await _teachingStaff.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(classes);
        }

        [HttpGet("teachingStaffByTeacherID")]
        public async Task<IActionResult> GetTeachingStaffByTeacherID(string TeacherID)
        {
            var teacher = await _teachingStaff.GetTeachingStaffByTeacherID(TeacherID);

            return Ok(teacher);
        }


        [HttpPut("addSubject")]
        public async Task<IActionResult> AssignSubjectByTeacherID(AddDataModel addSubjectModel)
        {
            var subject = new Subject
            {
                TenantId = _contextAccessor.HttpContext?.User.GetUserId()
            };
            var teacher = await _teachingStaff.AssignSubjectByTeacherID(addSubjectModel, subject);

            return Ok(teacher);
        }

        [HttpPut("addClass")]
        public async Task<IActionResult> AssignClassByTeacherID(AddDataModel addClassModel)
        {
            var @class = new Class
            {
                TenantId = _contextAccessor.HttpContext?.User.GetUserId()
            };

            var teacher = await _teachingStaff.AssignClassByTeacherID(addClassModel, @class);

            return Ok(teacher);
        }


        [HttpDelete("teacherID")]
        public async Task<IActionResult> DeleteTeachingByID(string TeacherID)
        {
            await _teachingStaff.DeleteTeachingByID(TeacherID);

            return NoContent();
        }

    }
}
