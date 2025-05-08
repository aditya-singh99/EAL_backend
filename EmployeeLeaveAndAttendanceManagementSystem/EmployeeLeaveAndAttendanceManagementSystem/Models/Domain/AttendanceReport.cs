using System.ComponentModel.DataAnnotations;

namespace EmployeeLeaveAndAttendanceManagementSystem.Models.Domain
{
    public class AttendanceReport

    {
        [Key]
        public int ReportID { get; set; }
        public int EmployeeID { get; set; }
        //public DateOnly StartDate { get; set; }
        //public DateOnly EndDate { get; set; }
        public int DateRange { get; set; } // Number of days in the report
        public int TotalAttendance { get; set; }
        public int Absenteeism { get; set; }

        public Employee Employee { get; set; } = null!;
    }
}
