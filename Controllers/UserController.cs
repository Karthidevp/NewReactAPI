using Microsoft.AspNetCore.Mvc;
using Reactapp.Interfaces;
using Reactapp.Models;
using System.Threading.Tasks;

namespace NewReact.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserservices _userService;

        public UserController(IUserservices userService)
        {
            _userService = userService;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { Success = false, Message = "User not found" });

            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserModels user)
        {
            var (success, message) = await _userService.CreateUserAsync(user);

            if (success)
                return Ok(new { Success = true, Message = message });

            return BadRequest(new { Success = false, Message = message });
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UserModels user)
        {
            if (user.UserId == 0)
                return BadRequest(new { Success = false, Message = "User ID is required" });

            var (success, message) = await _userService.UpdateUserAsync(user);

            if (success)
                return Ok(new { Success = true, Message = message });

            return BadRequest(new { Success = false, Message = message });
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var (success, message) = await _userService.DeleteUserAsync(id);

            if (success)
                return Ok(new { Success = true, Message = message });

            return NotFound(new { Success = false, Message = message });
        }
    }
}
