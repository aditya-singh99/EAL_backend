﻿using EmployeeLeaveAndAttendanceManagementSystem.Models.Domain;

namespace EmployeeLeaveAndAttendanceManagementSystem.Models.DTO
{
    public class AttendanceReportDTO
    {
        public int EmployeeId { get; set; }
        public int DateRange { get; set; } // Number of days in the report

    }
}
