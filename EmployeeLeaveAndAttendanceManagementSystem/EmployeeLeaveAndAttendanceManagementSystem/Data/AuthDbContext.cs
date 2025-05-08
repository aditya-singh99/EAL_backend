using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAndAttendanceManagementSystem.Data
{
        public class AuthDbContext : IdentityDbContext
        {
            public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
            {
            }

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder);

                var employeeRoleId = "38e75b6c-b8ec-4951-c481-946a2f8e8642";
                var managerRoleId = "a850f27d-35b2-5335-b8cf-2cc11c8d1234";
                var adminRoleId = "a850f27d-35b2-5335-b8cf-2cc11c8d5678";

                // Create Employee, HR, Manager, and Admin Roles
                var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = employeeRoleId,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE",
                    ConcurrencyStamp = employeeRoleId
                },
                new IdentityRole()
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = managerRoleId
                },
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = adminRoleId
                }
            };

                // Seed the roles
                builder.Entity<IdentityRole>().HasData(roles);

                // Create an Admin User
                var adminUserId = "f3d378fd-e54d-5f4c-9219-b2b2f92a017e";
                var admin = new IdentityUser()
                {
                    Id = adminUserId,
                    UserName = "admin@AttendanceAndLeave.com",
                    Email = "admin@AttendanceAndLeave.com",
                    NormalizedEmail = "ADMIN@ATTENDANCEANDLEAVE.COM",
                    NormalizedUserName = "ADMIN@ATTENDANCEANDLEAVE.COM"
                };

                admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Password@1234");

                builder.Entity<IdentityUser>().HasData(admin);

                // Assign Roles to Admin
                var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = employeeRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = managerRoleId
                },
                new()
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                }
            };

                builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);

                // Create an Employee User
                var employeeUserId = "b3d378fd-e54d-5f4c-9219-b2b2f92a017e";
                var employeeUser = new IdentityUser()
                {
                    Id = employeeUserId,
                    UserName = "employee@AttendanceAndLeave.com",
                    Email = "employee@AttendanceAndLeave.com",
                    NormalizedEmail = "EMPLOYEE@ATTENDANCEANDLEAVE.COM",
                    NormalizedUserName = "EMPLOYEE@ATTENDANCEANDLEAVE.COM"
                };
                employeeUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(employeeUser, "EmployeePassword@1234");
                builder.Entity<IdentityUser>().HasData(employeeUser);

                // Assign Employee Role to Employee User
                builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
                {
                    UserId = employeeUserId,
                    RoleId = employeeRoleId
                });

                // Create an HR User
               
                // Create a Manager User
                var managerUserId = "d3d378fd-e54d-5f4c-9219-b2b2f92a017e";
                var managerUser = new IdentityUser()
                {
                    Id = managerUserId,
                    UserName = "manager@AttendanceAndLeave.com",
                    Email = "manager@AttendanceAndLeave.com",
                    NormalizedEmail = "MANAGER@ATTENDANCEANDLEAVE.COM",
                    NormalizedUserName = "MANAGER@ATTENDANCEANDLEAVE.COM"
                };
                managerUser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(managerUser, "ManagerPassword@7890");
                builder.Entity<IdentityUser>().HasData(managerUser);

                // Assign Manager and Employee Roles to Manager User
                var managerUserRoles = new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    UserId = managerUserId,
                    RoleId = managerRoleId
                },
                new IdentityUserRole<string>
                {
                    UserId = managerUserId,
                    RoleId = employeeRoleId
                }
            };
                builder.Entity<IdentityUserRole<string>>().HasData(managerUserRoles);
            }
        }
    }


