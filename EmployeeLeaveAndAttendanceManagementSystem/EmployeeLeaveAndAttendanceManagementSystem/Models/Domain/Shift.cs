using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAndAttendanceManagementSystem.Models.Domain
{
    public class Shift
    {
        [Key]
        public int ShiftID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime ShiftDate { get; set; }
        public DateTime ShiftTime { get; set; }
        public Employee Employee { get; set; } = null!;

    }
}
