namespace EmployeeLeaveAndAttendanceManagementSystem.Models.DTO
{
    public class LeaveRequestDTO
    {
        public int EmployeeId { get; set; }
        public string LeaveType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        
    }
}
