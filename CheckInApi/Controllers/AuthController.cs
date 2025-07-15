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

        public AuthController(IAuthService authservice)
        {
            _authService = authservice;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserData userData)
        {
            // Ensure Id is not set explicitly
            userData.Id = 0;
            var result=_authService.RegistrationUser(userData);

            if (!result)
            {
                return BadRequest("Email already exists");
            }

            return Ok("Registered successfully");
        }


        [HttpPost("login")]
        public IActionResult Login(LoginDto logindata)
        {
            var data = _authService.LoginUser(logindata);
               

            if (data == null)
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
                    
                    
                    email = data.Email,
                    password = data.Password
                    // add other fields if needed
                }
            });
        }

    }
}
