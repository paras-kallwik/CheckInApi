using CheckInApi.Model;
using CheckInApi.Service;
using Microsoft.AspNetCore.Mvc;

namespace CheckInApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserData userData)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid user data");

            var result = await _authService.RegistrationUserAsync(userData);
            if (!result) return BadRequest("Email already exists");

            return Ok(new
            {
                status = "success",
                message = "Registered successfully"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto logindata)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid credentials");

            var user = await _authService.LoginUserAsync(logindata);
            if (user == null)
            {
                return Unauthorized(new
                {
                    status = "error",
                    message = "Invalid email or password"
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Login successful",
                user = new
                {
                    fname = user.Fname,
                    lname = user.Lname,
                    email = user.Email,
                    phone = user.Phone
                }
            });
        }
    }
}
