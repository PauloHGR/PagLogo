using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PagLogo.Models;
using PagLogo.Services;

namespace PagLogo.Controllers
{

    [ApiController]
    [Route("/api/v1/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;

        public UserController(ILogger<UserController> logger, IUserService userService, ITransactionService transactionService)
        {
            _logger = logger;
            _userService = userService;
            _transactionService = transactionService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<User> GetUsers([FromQuery] string identifier)
        {
            var result = await _userService.GetUserAsync(identifier);
            return result;
        }

        [HttpGet("all")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<List<User>> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
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

        [HttpGet("transaction")]
        public async Task CallTransactionAsync([FromQuery] TransactionFilterRequest request)
        {
            await _transactionService.CallTransactionAsync(request);
        }

    }
}
