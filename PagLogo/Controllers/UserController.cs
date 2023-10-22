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
            var result = await _userService.GetUsersAsync(identifier);
            return result;
        }
    }
}
