using Microsoft.AspNetCore.Mvc;
using PagLogo.Models;
using PagLogo.Services;

namespace PagLogo.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<User> GetUsers([FromQuery] string identifier)
        {
            var result = await _userService.GetUserAsync(identifier);
            return result;
        }

        [HttpPost]
        public async Task SaveUser([FromBody] User user)
        {
            await _userService.SaveUserAsync(user);
        }

        [HttpPut]
        public async Task UpdateUser([FromBody] User user)
        {
            await _userService.UpdateUserAsync(user);
        }

        [HttpDelete]
        public async Task DeleteUser([FromQuery] string identifier)
        {
            await _userService.DeleteUserAsync(identifier);
        }
    }
}
