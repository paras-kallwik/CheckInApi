using CheckInApi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CheckInApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly UserDbContext _context;

        public AttendanceController(UserDbContext context)
        {
            _context = context;
        }

        [HttpPost("mark")]
        public IActionResult MarkAttendance([FromBody] string email)
        {
            var user = _context.Usersdata.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user == null)
                return NotFound("User not found!");

            var today = DateTime.Now.Date;

            var alreadyMarked = _context.AttendanceRecords
                .Any(a => a.UserId == user.Id && a.Date == today);

            if (alreadyMarked)
                return BadRequest("Attendance already marked for today.");

            var record = new AttendanceRecord
            {
                UserId = user.Id,
                Email = user.Email,
                Date = today,
                Time = DateTime.Now.TimeOfDay,
                Status = "Present"
            };

            _context.AttendanceRecords.Add(record);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Attendance marked successfully",
                user = $"{user.Fname} {user.Lname}",
                email = user.Email,
                date = record.Date.ToShortDateString(),
                time = record.Time.ToString(),
                status = record.Status
            });
        }



        [HttpGet("{email}")]
        public IActionResult GetAttendance(string email)
        {
            var records = _context.AttendanceRecords
                .Include(a => a.User)  // Include user navigation property
                .Where(a => a.User.Email == email)
                .OrderByDescending(a => a.Date)
                .Select(a => new
                {
                    a.Date,
                    a.Time,
                    a.Status,
                    UserEmail = a.User.Email,
                    UserName = a.User.Fname + " " + a.User.Lname
                })
                .ToList();

            if (records == null || records.Count == 0)
                return NotFound("No attendance records found for this email.");

            return Ok(records);
        }

    }
}
