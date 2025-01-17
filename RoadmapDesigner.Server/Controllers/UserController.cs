using Microsoft.AspNetCore.Mvc;
using RoadmapDesigner.Server.Interfaces;
using RoadmapDesigner.Server.Models.DTO;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IUserService userService, ILogger<UserController> logger) 
        {
            _logger = logger;
            _userService = userService;
        }
        [HttpGet("user")]
        public async Task<ActionResult<UserDTO>> GetUserByID(Guid userID)
        {
            var user = await _userService.GetUserByGuidAsync(userID);

            return Ok(user);
        }

        [HttpGet("users")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }
    }
}
