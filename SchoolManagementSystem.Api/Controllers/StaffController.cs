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
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SchoolManagementSystem.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaff _Staff;

        private readonly ITenantResolver _tenantService;

        private readonly IHttpContextAccessor _contextAccessor;

        public StaffController(IStaff Staff, IHttpContextAccessor contextAccessor, ITenantResolver tenantService)
        {
            _Staff = Staff;
            _contextAccessor = contextAccessor;
            _tenantService = tenantService;
        }

        [HttpPost("addingTeachingStaff")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> AddingTeachingStaff([FromForm]StaffModel StaffModel)
        {

            var staff = await _Staff.AddingStaff(StaffModel);

            return CreatedAtAction(nameof(GetStaffByTeacherID), new { selectStaffModel = new SelectStaffModel() { StaffID = staff.StaffID , StaffCategory = StaffModel.StaffCategory} }, staff);
        }

        [HttpPut("updateTeachingStaff")]
        public async Task<IActionResult> UpdateTeachingStaff([Required]SelectStaffModel selectStaffModel, [FromForm] StaffModel StaffModel)
        {
            var updatedteacher = await _Staff.UpdateTeachingStaff(selectStaffModel, StaffModel);

            return Ok(updatedteacher);

        }

        [HttpGet("SortingTeachingStaff")]
        public async Task<IActionResult> SortingTeachingStaff([FromQuery] SortingTeachingStaffModel sortingTeachingStaff)
        {

            var tenancyId = _contextAccessor.HttpContext.User.GetUserId();

            var staffs = await _Staff.SortingTeachingStaff(sortingTeachingStaff, tenancyId);

            return Ok(staffs);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search([FromQuery] string searchquery)
        {

            var tenancyId = _contextAccessor.HttpContext.User.GetUserId();

            var staffs = await _Staff.SearchFuntion(searchquery, tenancyId);

            return Ok(staffs);
        }



        [HttpGet("allTeachingStaffWithClassAndSubjectOnly")]
        public async Task<IActionResult> GetAllTeachingStaffWithClassAndSubjectOnly()
        {
            var teachers = await _Staff.GetAllTeachingStaffWithClassAndSubjectOnly();

            return Ok(teachers);
        }

        [HttpGet("AllNonTeachingStaff")]
        public async Task<IActionResult> GetAllNonTeachingStaff()
        {
            var nonTeachers = await _Staff.GetAllNonTeachingStaff();

            return Ok(nonTeachers);
        }



        [HttpGet("allSubjectOfTeacherByTeacherID")]
        public async Task<IActionResult> GetAllSubjectOfTeacherByTeacherID([Required] string TeacherID)
        {
            var subjects = await _Staff.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(subjects);
        }

        [HttpGet("allClassesOfTeacherByTeacherID")]
        public async Task<IActionResult> GetAllClassOfTeacherByTeacherID([Required] string TeacherID)
        {
            var classes = await _Staff.GetAllSubjectOfTeacherByTeacherID(TeacherID);

            return Ok(classes);
        }

        [HttpGet("teachingStaffByTeacherID")]
        public async Task<IActionResult> GetStaffByTeacherID([Required] SelectStaffModel selectStaffModel)
        {
            var teacher = await _Staff.GetStaffByStaffID(selectStaffModel);

            return Ok(teacher);
        }


        [HttpPut("addSubject")]
        public async Task<IActionResult> AssignSubjectByTeacherID(AddDataModel addSubjectModel)
        {
           
            var teacher = await _Staff.AssignSubjectByTeacherID(addSubjectModel);

            return Ok(teacher);
        }

        [HttpPut("addClass")]
        public async Task<IActionResult> AssignClassByTeacherID(AddDataModel addClassModel)
        {
            
            var teacher = await _Staff.AssignClassByTeacherID(addClassModel);

            return Ok(teacher);
        }

      

        [HttpDelete("teacherID")]
        public async Task<IActionResult> DeleteStaffByID(SelectStaffModel selectStaffModel)
        {
            await _Staff.DeleteStaffByID(selectStaffModel);

            return NoContent();
        }

    }
}
