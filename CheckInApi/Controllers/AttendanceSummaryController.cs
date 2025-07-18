using Azure.Data.Tables;
using CheckInApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace CheckInApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceSummaryController : ControllerBase
    {
        private readonly TableClient _attendanceTable;

        public AttendanceSummaryController(IConfiguration configuration)
        {
            string connectionString = configuration["AzureTableStorage:ConnectionString"];
            _attendanceTable = new TableClient(connectionString, "AttendanceRecords");
        }

        [HttpGet("get-attendance")]
        public IActionResult GetAttendanceSummary([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required");

            // Get records where PartitionKey == email
            var records = _attendanceTable
                .Query<AttendanceRecord>(r => r.PartitionKey == email.ToLower())
                .ToList();

            if (records == null || records.Count == 0)
                return NotFound("No attendance records found for this email.");

            var attendanceSummaries = records
                .GroupBy(r => r.Date.Date)
                .Select(g => new AttendanceSummaryDto
                {
                    Email = email.ToLower(),
                    Date = g.Key,
                    CheckInTime = g.FirstOrDefault(x => x.Status == "CheckIn")?.Time.ToString(@"hh\:mm\:ss"),
                    CheckOutTime = g.FirstOrDefault(x => x.Status == "CheckOut")?.Time.ToString(@"hh\:mm\:ss")
                })
                .OrderBy(a => a.Date)
                .ToList();

            return Ok(attendanceSummaries);
        }
    }
}
