using Microsoft.AspNetCore.Mvc;
using CheckInApi.Service;

namespace CheckInApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        [HttpPost("checkin")]
        public async Task<IActionResult> CheckIn([FromBody] string email)
        {
            var result = await _attendanceService.MarkAttendanceAsync(email, "CheckIn");
            return result.StartsWith("Successfully") ? Ok(result) : BadRequest(result);
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> CheckOut([FromBody] string email)
        {
            var result = await _attendanceService.MarkAttendanceAsync(email, "CheckOut");
            return result.StartsWith("Successfully") ? Ok(result) : BadRequest(result);
        }
    }
}
