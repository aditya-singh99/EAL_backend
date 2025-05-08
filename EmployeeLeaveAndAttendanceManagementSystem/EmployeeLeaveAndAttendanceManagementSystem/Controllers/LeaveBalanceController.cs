using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EmployeeLeaveAndAttendanceManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using EmployeeLeaveAndAttendanceManagementSystem.Models.DTO;
using EmployeeLeaveAndAttendanceManagementSystem.Models.Domain;
using Microsoft.AspNetCore.Authorization;
namespace EmployeeLeaveAndAttendanceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveBalanceController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public LeaveBalanceController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpGet]
        [Route("GetAllLeaveBalances")]
        [Authorize(Roles = "Admin,Employee,Manager")]
        public async Task<IActionResult> GetAllLeavebalances()
        {
            var allLeaveBalances = await dbContext.LeaveBalances.ToListAsync();
            return Ok(allLeaveBalances);
        }

        [HttpGet]
        [Route("GetLeaveBalanceById/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetLeaveBalanceById(int id)
        {
            var leavebalance = await dbContext.LeaveBalances.FindAsync(id);
            if (leavebalance == null)
            {
                return NotFound("Leave balance not found");
            }

            return Ok(leavebalance);
        }


        [HttpPost]
        [Route("AddLeaveBalance")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddLeaveBalance([FromBody] LeaveBalanceDTO leavebalancedto)
        {
            var tempEmployee = await dbContext.Employees.FirstOrDefaultAsync(a => a.EmployeeID == leavebalancedto.EmployeeId);

            if (tempEmployee == null)
            {
                var errormessage = "This Employee with the ID : " + leavebalancedto.EmployeeId + "is not a in the database. Please add the employee first.";
                return BadRequest(errormessage);
            }
            if (leavebalancedto == null)
            {
                return BadRequest("Leave request cannot be null");
            }

            var leavebalanceDB = new LeaveBalance();
            leavebalanceDB.EmployeeID = leavebalancedto.EmployeeId;
            leavebalanceDB.LeaveType = leavebalancedto.LeaveType;
            leavebalanceDB.Balance = leavebalancedto.Balance;

            await dbContext.LeaveBalances.AddAsync(leavebalanceDB);
            await dbContext.SaveChangesAsync();
            return Ok(leavebalancedto);
        }

        [HttpPut]
        [Route("UpdateLeaveBalance/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateLeaveBalance(int id, [FromBody] UpdateLeaveBalanceDTO updateleavebalancedto)
        {
            if (updateleavebalancedto == null)
            {
                return BadRequest("Leave balance data cannot be null");
            }

            var leavebalance = await dbContext.LeaveBalances.FindAsync(id);
            if (leavebalance == null)
            {
                return NotFound("Leave balance not found");
            }

            
            leavebalance.LeaveType = updateleavebalancedto.LeaveType;
            leavebalance.Balance = updateleavebalancedto.Balance;

            dbContext.LeaveBalances.Update(leavebalance);
            await dbContext.SaveChangesAsync();
            return Ok(updateleavebalancedto);
        }

        [HttpDelete]
        [Route("DeleteLeaveBalance/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteLeaveBalance(int id)
        {
            var leavebalance = await dbContext.LeaveBalances.FindAsync(id);
            if (leavebalance == null)
            {
                return NotFound("Leave balance not found");
            }

            dbContext.LeaveBalances.Remove(leavebalance);
            await dbContext.SaveChangesAsync();
            return Ok("Leave balance deleted successfully");
        }


    }
}
