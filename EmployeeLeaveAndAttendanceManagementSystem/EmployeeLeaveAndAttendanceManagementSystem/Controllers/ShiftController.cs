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
    public class ShiftController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public ShiftController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpPost]
        [Route("AddShift")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AddShift([FromBody] ShiftDTO shiftdto)
        {
            var tempShift = await dbContext.Employees.FirstOrDefaultAsync(a => a.EmployeeID == shiftdto.EmployeeId);

            if (tempShift == null)
            {
                var errormessage = "This Employee with the ID : " + shiftdto.EmployeeId + "is not a in the database. Please add the employee first.";
                return BadRequest(errormessage);
            }

            var shiftDB = new Shift();
            shiftDB.EmployeeID = shiftdto.EmployeeId;
            shiftDB.ShiftDate = shiftdto.ShiftDate;
            shiftDB.ShiftTime = shiftdto.ShiftTime;

            if (shiftdto == null)
            {
                return BadRequest("Leave request cannot be null");
            }

            await dbContext.Shifts.AddAsync(shiftDB);
            await dbContext.SaveChangesAsync();
            return Ok(shiftdto);
        }

        [HttpGet]
        [Route("GetAllShift")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllShift()
        {
            var shifts = await dbContext.Shifts.ToListAsync();
            return Ok(shifts);
        }

        [HttpGet]
        [Route("GetShiftById/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetShiftById(int id)
        {
            var shift = await dbContext.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound("Shift not found");
            }

            return Ok(shift);
        }
        


        [HttpPut]
        [Route("UpdateShift/{id}")]
        [Authorize(Roles = "Admin,Manager")]

        public async Task<IActionResult> UpdateShift(int id, [FromBody] UpdateShiftDTO updateshiftdto)
        {
            if (updateshiftdto == null)
            {
                return BadRequest("Shift request cannot be null");
            }

            var shift = await dbContext.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound("Shift not found");
            }

            shift.ShiftDate = updateshiftdto.ShiftDate;
            shift.ShiftTime = updateshiftdto.ShiftTime;

            dbContext.Shifts.Update(shift);
            await dbContext.SaveChangesAsync();
            return Ok(updateshiftdto);
        }

        [HttpDelete]
        [Route("DeleteShift/{id}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> DeleteShift(int id)
        {
            var shift = await dbContext.Shifts.FindAsync(id);
            if (shift == null)
            {
                return NotFound("Shift not found");
            }

            dbContext.Shifts.Remove(shift);
            await dbContext.SaveChangesAsync();
            return Ok("Shift deleted successfully");
        }
    
    }
}
