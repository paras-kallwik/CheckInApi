using System;

namespace CheckInApi.Model
{
    public class AttendanceSummaryDto
    {
        public string Email { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public string? CheckInTime { get; set; }  // formatted like "09:00 AM"
        public string? CheckOutTime { get; set; } // formatted like "06:00 PM"
    }
}
