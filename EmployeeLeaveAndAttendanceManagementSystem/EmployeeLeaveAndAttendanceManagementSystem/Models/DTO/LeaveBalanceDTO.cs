using EmployeeLeaveAndAttendanceManagementSystem.Models.Domain;

namespace EmployeeLeaveAndAttendanceManagementSystem.Models.DTO
{
    public class LeaveBalanceDTO
    {
        public int EmployeeId { get; set; }
        public string LeaveType { get; set; }
        public int Balance { get; set; }
    }
}
