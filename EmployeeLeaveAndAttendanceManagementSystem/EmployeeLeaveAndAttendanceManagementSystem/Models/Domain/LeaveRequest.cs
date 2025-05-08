using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAndAttendanceManagementSystem.Models.Domain
{
    public class LeaveRequest
    {
        public LeaveRequest()
        {
            this.Status = "Pending";
        }
        [Key]
        public int LeaveID { get; set; }
        public int EmployeeID { get; set; }
        public string LeaveType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
