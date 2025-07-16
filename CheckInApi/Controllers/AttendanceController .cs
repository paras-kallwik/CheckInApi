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

        [HttpPost("checkin")]
        public IActionResult MarkInAttendance([FromBody] string email)
        {
            return MarkAttendance(email, "CheckIn");
        }

        [HttpPost("checkout")]
        public IActionResult MarkOutAttendance([FromBody] string email)
        {
            return MarkAttendance(email, "CheckOut");
        }

        // ✅ Common reusable method
        private IActionResult MarkAttendance(string email, string status)
        {
            var user = _context.Usersdata.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
                return NotFound("User not found!");

            var today = DateTime.Now.Date;

            if (status == "CheckIn")
            {
                var alreadyMarked = _context.AttendanceRecords
                    .Any(a => a.UserId == user.Id && a.Date == today && a.Status == "CheckIn");

                if (alreadyMarked)
                    return BadRequest("Already checked in for today.");
            }
            else if (status == "CheckOut")
            {
                var checkInRecord = _context.AttendanceRecords
                    .FirstOrDefault(a => a.UserId == user.Id && a.Date == today && a.Status == "CheckIn");

                if (checkInRecord == null)
                    return BadRequest("You haven't checked in yet today.");

                var alreadyCheckedOut = _context.AttendanceRecords
                    .Any(a => a.UserId == user.Id && a.Date == today && a.Status == "CheckOut");

                if (alreadyCheckedOut)
                    return BadRequest("You have already checked out for today.");
            }

            var record = new AttendanceRecord
            {
                UserId = user.Id,
                Email = user.Email,
                Date = today,
                Time = DateTime.Now.TimeOfDay,
                Status = status
            };

            _context.AttendanceRecords.Add(record);
            _context.SaveChanges();

            return Ok(new
            {
                message = $"Successfully marked {status}",
                user = $"{user.Fname} {user.Lname}",
                email = user.Email,
                date = record.Date.ToShortDateString(),
                time = record.Time.ToString(@"hh\:mm\:ss"),
                status = record.Status
            });
        }
    }
}

    
