namespace EmployeeLeaveAndAttendanceManagementSystem.Models.DTO
{
    public class UpdateLeaveRequestDTO
    {
        public string LeaveType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Status { get; set; }
    }
}
