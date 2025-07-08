using CheckInApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace CheckInApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserDbContext _userDbContext;

        public AuthController(UserDbContext userDbContext)
        {
            _userDbContext = userDbContext;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public IActionResult Registration([FromBody] UserData userData)
        {
            // Ensure Id is not set explicitly
            userData.Id = 0;

            if (_userDbContext.Usersdata.Any(u => u.Email == userData.Email))
            {
                return BadRequest("Email already exists");
            }

            _userDbContext.Usersdata.Add(userData);
            _userDbContext.SaveChanges();

            return Ok("Registered successfully");
        }
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto logindata)
        {
            var data = _userDbContext.Usersdata
                .FirstOrDefault(u => u.Email == logindata.Email && u.Password == logindata.Password);

            if (data == null)
            {
                return Unauthorized("Invalid email or password");
            }

            return Ok("Login successful");
        }

    }
}
