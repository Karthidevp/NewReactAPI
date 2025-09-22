using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reactapp.Interfaces;
using Reactapp.Models;

namespace NewReact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILoginServices _userService;

        public AuthController(ILoginServices userService)
        {
            _userService = userService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (login == null || string.IsNullOrEmpty(login.EmailId) || string.IsNullOrEmpty(login.Password))
                return BadRequest("Email and Password are required.");

            var result = await _userService.LoginUserAsync(login);

            return Ok(result);

          
        }
    }
}
