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
    public class AttendanceReportController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public AttendanceReportController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpPost]
        [Route("AddAttendanceReport")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddAttendanceReport([FromBody] AttendanceReportDTO attendancereportdto)
        {
            var tempAttendanceReport = await dbContext.Employees.FirstOrDefaultAsync(a => a.EmployeeID == attendancereportdto.EmployeeId);

            if (tempAttendanceReport == null)
            {
                var errormessage = "This Employee with the ID : " + attendancereportdto.EmployeeId + "is not a in the database. Please add the employee first.";
                return BadRequest(errormessage);
            }

            if (attendancereportdto == null)
            {
                return BadRequest("Attendance Report request cannot be null");
            }

            var attendancereportDB = new AttendanceReport();
            attendancereportDB.EmployeeID = attendancereportdto.EmployeeId;
            //attendancereportDB.StartDate = attendancereportdto.StartDate;
            //attendancereportDB.EndDate = attendancereportdto.EndDate;
            attendancereportDB.DateRange = attendancereportdto.DateRange;

            await dbContext.AttendanceReports.AddAsync(attendancereportDB);
            await dbContext.SaveChangesAsync();
            return Ok(attendancereportdto);
        }

        [HttpGet]
        [Route("GetAllAttendanceReport")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllAttendanceReport()
        {
            var attendancereports = await dbContext.AttendanceReports.ToListAsync();
            return Ok(attendancereports);
        }

        [HttpGet]
        [Route("GetAttendanceReportById/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAttendanceReportById(int id)
        {
            var attendancereport = await dbContext.AttendanceReports.FindAsync(id);
            if (attendancereport == null)
            {
                return NotFound("Attendance Report not found.");
            }

            AttendanceReportDTO attendanceReportDTO = new AttendanceReportDTO();
            attendanceReportDTO.EmployeeId = attendancereport.EmployeeID;
            //attendanceReportDTO.StartDate = attendancereport.StartDate;
            //attendanceReportDTO.EndDate = attendancereport.EndDate;
            attendanceReportDTO.DateRange = attendancereport.DateRange;


            return Ok(attendanceReportDTO); // we only sent employee id and date range to the client
        }


        [HttpPut]
        [Route("UpdateAttendanceReport/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateAttendanceReport(int id, [FromBody] UpdateAttendanceReportDTO attendancereportdto)
        {
            var existingattendancereport = await dbContext.AttendanceReports.FindAsync(id);
            if (existingattendancereport == null)
            {
                return NotFound($"attendance report with id {id} not found.");
            }

            //existingattendancereport.StartDate = attendancereportdto.StartDate;
            //existingattendancereport.EndDate = attendancereportdto.EndDate;
            existingattendancereport.DateRange = attendancereportdto.DateRange;

            await dbContext.SaveChangesAsync();

            return Ok(existingattendancereport);
            //return Ok("Endpoint functionality yet to be implemented");
        }


        [HttpDelete]
        [Route("DeleteAttendanceReport/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAttendanceReport(int id)
        {
            var existingattendancereport = await dbContext.AttendanceReports.FindAsync(id);
            if (existingattendancereport == null)
            {
                return NotFound($"attendance report with id {id} not found.");
            }
            dbContext.AttendanceReports.Remove(existingattendancereport);
            await dbContext.SaveChangesAsync();
            return Ok("Delete report successfully");
        }

    }
}
