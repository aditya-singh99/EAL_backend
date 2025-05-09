﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EmployeeLeaveAndAttendanceManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("CreateRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name cannot be empty");
            }

            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (roleResult.Succeeded)
                {
                    return Ok($"Role {roleName} created successfully");
                }
                return StatusCode(500, "Error creating role");
            }
            return BadRequest("Role already exists");
        }

        [HttpPost]
        [Route("AssignRole")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole([FromBody] UserRoleDTO userRoleDto)
        {
            var user = await userManager.FindByEmailAsync(userRoleDto.Email);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roleExist = await roleManager.RoleExistsAsync(userRoleDto.Role);
            if (!roleExist)
            {
                return BadRequest("Role does not exist");
            }

            var result = await userManager.AddToRoleAsync(user, userRoleDto.Role);
            if (result.Succeeded)
            {
                return Ok($"Role {userRoleDto.Role} assigned to user {userRoleDto.Email} successfully");
            }
            return StatusCode(500, "Error assigning role");
        }
    }

    public class UserRoleDTO
    {
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
