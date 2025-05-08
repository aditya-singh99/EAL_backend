using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAndAttendanceManagementSystem.Models.Domain
{
    public class LeaveBalance
    {
        [Key]
        public int LeaveBalanceID { get; set; }
        public int EmployeeID { get; set; } 
        public string LeaveType { get; set; }
        public int Balance { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
