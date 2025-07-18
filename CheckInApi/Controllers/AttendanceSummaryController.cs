using Azure;
using Azure.Data.Tables;
using CheckInApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace CheckInApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceSummaryController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;
        private readonly IConfiguration _configuration;

        public AttendanceSummaryController(UserDbContext userDbContext, IConfiguration configuration)
        {
            _userDbContext = userDbContext;
            _configuration = configuration;
        }

        [HttpGet("get-attendance")]
        public IActionResult GetAttendanceSummary([FromQuery] string email)
        {
            var user = _userDbContext.Usersdata.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
                return NotFound("User not found!");

            string connectionString = _configuration.GetConnectionString("AzureTableStorage");
            string tableName = "Attendance";

            var tableClient = new TableClient(connectionString, tableName);

            // Fetch all attendance records for the user from Table Storage
            var records = tableClient.Query<AttendanceRecord>(r => r.PartitionKey == user.PartitionKey.ToString()).ToList();

            var attendanceSummaries = records
                .GroupBy(r => r.Date.Date) // Group by date only (ignore time)
                .Select(g => new AttendanceSummaryDto
                {
                    Email = user.Email,
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
