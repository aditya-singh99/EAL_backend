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
    public class LeaveRequestController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public LeaveRequestController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpPost]
        [Route("AddLeave")]
        [Authorize(Roles = "Admin,Employee,Manager")]
        public async Task<IActionResult> AddLeave([FromBody] LeaveRequestDTO leaverequestdto)
        {
            var tempEmployee = await dbContext.Employees.FirstOrDefaultAsync(a => a.EmployeeID == leaverequestdto.EmployeeId);

            if (tempEmployee == null) {
                var errormessage = "This Employee with the ID : " + leaverequestdto.EmployeeId + "is not a in the database. Please add the employee first.";
                return BadRequest(errormessage);
            }

            if (leaverequestdto == null)
            {
                return BadRequest("Leave request cannot be null");
            }

            var leaverequestDB = new LeaveRequest();
            leaverequestDB.EmployeeID = leaverequestdto.EmployeeId;
            leaverequestDB.LeaveType = leaverequestdto.LeaveType;
            leaverequestDB.StartDate = leaverequestdto.StartDate;
            leaverequestDB.EndDate = leaverequestdto.EndDate;

            await dbContext.LeaveRequests.AddAsync(leaverequestDB);
            await dbContext.SaveChangesAsync();
            return Ok(leaverequestdto);
        }

        [HttpGet]
        [Route("GetAllLeaves")]
        [Authorize(Roles = "Admin,Employee,Manager")]
        public async Task<IActionResult> GetAllLeaves()
        {
            var leaverequests = await dbContext.LeaveRequests.ToListAsync();
            return Ok(leaverequests);
        }

        [HttpGet]
        [Route("GetLeaveById/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetLeaveById(int id)
        {
            var leaveRequest = await dbContext.LeaveRequests.FindAsync(id);
            if (leaveRequest == null)
            {
                return NotFound("Leave request not found.");
            }

            return Ok(leaveRequest);
        }

        [HttpPut]
        [Route("UpdateLeave/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> UpdateLeave(int id, [FromBody] UpdateLeaveRequestDTO updateleaverequestdto)
        {
            if (updateleaverequestdto == null)
            {
                return BadRequest("Leave request cannot be null");
            }

            var leaverequest = await dbContext.LeaveRequests.FindAsync(id);
            if (leaverequest == null)
            {
                return NotFound("Leave request not found");
            }

            leaverequest.LeaveType = updateleaverequestdto.LeaveType;
            leaverequest.StartDate = updateleaverequestdto.StartDate;
            leaverequest.EndDate = updateleaverequestdto.EndDate;

            dbContext.LeaveRequests.Update(leaverequest);
            await dbContext.SaveChangesAsync();
            return Ok(updateleaverequestdto);
        }

        [HttpDelete]
        [Route("DeleteLeave/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            var leaverequest = await dbContext.LeaveRequests.FindAsync(id);
            if (leaverequest == null)
            {
                return NotFound("Leave request not found");
            }

            dbContext.LeaveRequests.Remove(leaverequest);
            await dbContext.SaveChangesAsync();
            return Ok("Leave request deleted successfully");
        }

    }
}
