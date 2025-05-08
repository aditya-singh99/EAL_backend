using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAndAttendanceManagementSystem.Models.Domain
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public TimeSpan WorkHours{ get; set; }
        public Employee Employee { get; set; } = null!;
        public void CalculateWorkHours()
        {
            WorkHours = ClockOutTime - ClockInTime;
        }
    }

}
