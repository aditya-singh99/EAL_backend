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
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public EmployeeController(ApplicationDbContext context)
        {
            this.dbContext = context;
        }

        [HttpPost]
        [Route("AddEmployee")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeDTO employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data fields cannot be null");
            }

            var employeeobjforDB = new Employee();
            employeeobjforDB.Name = employeeDto.Name;
            employeeobjforDB.Email = employeeDto.Email;
            employeeobjforDB.Address = employeeDto.Address;

            await dbContext.Employees.AddAsync(employeeobjforDB);
            await dbContext.SaveChangesAsync();
            return Ok(employeeDto);
        }

        [HttpGet]
        [Route("GetAllEmployees")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await dbContext.Employees.ToListAsync();
            List<GetEmployeeDTO> getEmployeeDTO = new List<GetEmployeeDTO>();

            if (employees == null)
            {
                return BadRequest("No employees found");
            }
            foreach (var emp in employees)
            {
                GetEmployeeDTO temp = new GetEmployeeDTO();
                temp.Id = emp.EmployeeID;
                temp.Name = emp.Name;
                temp.Email = emp.Email;
                temp.Address = emp.Address;
                getEmployeeDTO.Add(temp);
            }
            return Ok(getEmployeeDTO);
        }

        [HttpGet]
        [Route("GetEmployeeById/{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            GetEmployeeDTO getEmployeeDTO = new GetEmployeeDTO();
            getEmployeeDTO.Id = employee.EmployeeID;
            getEmployeeDTO.Name = employee.Name;
            getEmployeeDTO.Email = employee.Email;
            getEmployeeDTO.Address = employee.Address;

            return Ok(getEmployeeDTO);
        }

        [HttpPut]
        [Route("UpdateEmployee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeDTO employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Employee data fields cannot be null");
            }

            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            employee.Name = employeeDto.Name;
            employee.Email = employeeDto.Email;
            employee.Address = employeeDto.Address;

            dbContext.Employees.Update(employee);
            await dbContext.SaveChangesAsync();
            return Ok(employeeDto);
        }

        [HttpDelete]
        [Route("DeleteEmployee/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await dbContext.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound("Employee not found");
            }

            dbContext.Employees.Remove(employee);
            await dbContext.SaveChangesAsync();
            return Ok("Employee deleted successfully");
        }

    }
}
