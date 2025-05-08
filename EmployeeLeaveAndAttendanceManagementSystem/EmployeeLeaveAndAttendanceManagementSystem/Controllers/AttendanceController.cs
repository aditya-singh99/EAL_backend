using EmployeeLeaveAndAttendanceManagementSystem.Data;
using EmployeeLeaveAndAttendanceManagementSystem.Models.Domain;
using EmployeeLeaveAndAttendanceManagementSystem.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAndAttendanceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public AttendanceController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpPost]
        [Route("AddAttendance")]
        [Authorize(Roles = "Admin,Employee,Manager")]
        public async Task<IActionResult> AddAttendance([FromBody] AttendanceDTO attendancedto)
        {
            var tempAttendance = await dbContext.Employees.FirstOrDefaultAsync(a => a.EmployeeID == attendancedto.EmployeeId);

            if (tempAttendance == null)
            {
                var errormessage = "This Employee with the ID : " + attendancedto.EmployeeId + "is not a in the database. Please add the employee first.";
                return BadRequest(errormessage);
            }

            var attendanceDB = new Attendance();
            attendanceDB.EmployeeID = attendancedto.EmployeeId;
            attendanceDB.ClockInTime = attendancedto.ClockInTime;
            attendanceDB.ClockOutTime = attendancedto.ClockOutTime;

            if (attendancedto == null)
            {
                return BadRequest("Leave request cannot be null");
            }

            attendanceDB.CalculateWorkHours();
            await dbContext.Attendances.AddAsync(attendanceDB);
            await dbContext.SaveChangesAsync();
            return Ok(attendancedto);
        }

        [HttpGet]
        [Route("GetAllAttendance")]
        [Authorize(Roles = "Admin,Employee,Manager")]

        public async Task<IActionResult> GetAllAttendance()
        {
            var attendances = await dbContext.Attendances.ToListAsync();
            return Ok(attendances);
        }


        [HttpGet]
        [Route("GetAttendanceById/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAttendanceById(int id)
        {
            var attendance = await dbContext.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound("Attendance not found");
            }

            return Ok(attendance);

        }
        [HttpPut]
        [Route("UpdateAttendance/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateAttendance(int id, [FromBody] UpdateAttendanceDTO updateattendancedto)
        {
            var attendance = await dbContext.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound($"Attendance record with Id {id} not found.");
            }

            attendance.ClockInTime = updateattendancedto.ClockInTime;
            attendance.ClockOutTime = updateattendancedto.ClockOutTime;

            attendance.CalculateWorkHours();
            await dbContext.SaveChangesAsync();

            return Ok(attendance);
        }

        //delete attendance by id
        [HttpDelete]
        [Route("DeleteAttendanceById/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployeeById(int id)
        {
            var attendance = await dbContext.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound("Attendance not found");
            }
            dbContext.Attendances.Remove(attendance);
            await dbContext.SaveChangesAsync();
            return Ok("Attendance deleted successfully");
        }
    }
}

