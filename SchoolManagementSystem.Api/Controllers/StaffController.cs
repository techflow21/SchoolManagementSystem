using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolManagementSystem.Core.DTOs.Requests;
using SchoolManagementSystem.Api.Extensions;
using SchoolManagementSystem.Core.Entities;
using SchoolManagementSystem.Core.Interfaces;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaff _teachingStaff;

        private readonly ITenantResolver _tenantService;

        private readonly IHttpContextAccessor _contextAccessor;

        public StaffController(IStaff teachingStaff, IHttpContextAccessor contextAccessor, ITenantResolver tenantService)
        {
            _teachingStaff = teachingStaff;
            _contextAccessor = contextAccessor;
            _tenantService = tenantService;
        }

        [HttpPost("addingTeachingStaff")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddingTeachingStaff([FromForm]StaffModel teachingStaff)
        {

            var teacher = await _teachingStaff.AddingStaff(teachingStaff);

            return CreatedAtAction(nameof(GetTeachingStaffByTeacherID), new { teacher.id }, teacher);
        }

        [HttpPut("updateTeachingStaff")]
        public async Task<IActionResult> UpdateTeachingStaff([Required]string TeacherID, [FromForm] StaffModel teachingStaffModel)
        {
            var updatedteacher = await _teachingStaff.UpdateTeachingStaff(TeacherID, teachingStaffModel);

            return Ok(updatedteacher);

        }

        [HttpGet("SortingTeachingStaff")]
        public async Task<IActionResult> SortingTeachingStaff([FromQuery] SortingTeachingStaffModel sortingTeachingStaff)
        {

            var tenancyId = _contextAccessor.HttpContext.User.GetUserId();

            var teachers = await _teachingStaff.SortingTeachingStaff(sortingTeachingStaff, tenancyId);

            return Ok(teachers);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string searchquery)
        {

            var tenancyId = _contextAccessor.HttpContext.User.GetUserId();

            var teachers = await _teachingStaff.SearchFuntion(searchquery, tenancyId);

            return Ok(teachers);
        }



        [HttpGet("allTeachingStaffWithClassAndSubjectOnly")]
        public async Task<IActionResult> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _teachingStaff.GetAllTeachingStaffWithClassAndSubjectOnly();

            return Ok(teachers);
        }


        [HttpGet("allSubjectOfTeacherByTeacherID")]
        public async Task<IActionResult> GetAllSubjectOfTeacherByTeacherID([Required] string TeacherID)
        {
            var subjects = await _teachingStaff.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(subjects);
        }

        [HttpGet("allClassesOfTeacherByTeacherID")]
        public async Task<IActionResult> GetAllClassOfTeacherByTeacherID([Required] string TeacherID)
        {
            var classes = await _teachingStaff.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(classes);
        }

        [HttpGet("teachingStaffByTeacherID")]
        public async Task<IActionResult> GetTeachingStaffByTeacherID([Required] string TeacherID)
        {
            var teacher = await _teachingStaff.GetStaffByStaffID(TeacherID);

            return Ok(teacher);
        }


        [HttpPut("addSubject")]
        public async Task<IActionResult> AssignSubjectByTeacherID(AddDataModel addSubjectModel)
        {
           
            var teacher = await _teachingStaff.AssignSubjectByTeacherID(addSubjectModel);

            return Ok(teacher);
        }

        [HttpPut("addClass")]
        public async Task<IActionResult> AssignClassByTeacherID(AddDataModel addClassModel)
        {
            
            var teacher = await _teachingStaff.AssignClassByTeacherID(addClassModel);

            return Ok(teacher);
        }

      

        [HttpDelete("teacherID")]
        public async Task<IActionResult> DeleteTeachingByID(string TeacherID)
        {
            await _teachingStaff.DeleteStaffByID(TeacherID);

            return NoContent();
        }

    }
}
