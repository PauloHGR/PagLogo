using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using PagLogo.Exceptions;
using PagLogo.Models;
using PagLogo.Services;
using System.IdentityModel.Tokens.Jwt;

namespace PagLogo.Controllers
{

    [ApiController]
    [Route("/api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
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

        [HttpGet("{identifier}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetUsers(string identifier)
        {
            var result = await _userService.GetUserAsync(identifier);
            return this.Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserFilterRequest request)
        {
            var result = await _userService.GetAllUsers(request);
            return this.Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> SaveUser([FromBody] User user)
        {
            await _userService.SaveUserAsync(user);
            return this.Created("User",user);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            await _userService.UpdateUserAsync(user);
            return this.NoContent();
        }

        [HttpDelete]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteUser([FromQuery] string identifier)
        {
            await _userService.DeleteUserAsync(identifier);
            return this.NoContent();
        }

        [HttpGet("transaction")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CallTransactionAsync([FromQuery] TransactionFilterRequest request)
        {
            await _transactionService.CallTransactionAsync(request);
            return this.NoContent();
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> LoginAsync([FromQuery] LoginRequest request)
        {
            var tokenResult = await _userService.AuthenticateUser(request);

           try
            {
                return Ok(new { 
                    token = new JwtSecurityTokenHandler().WriteToken(tokenResult),
                    expiration = tokenResult.ValidTo
                });
            }
            catch (AuthenticateException ex)
            {
                return Unauthorized(ex);
            }
            
        }

    }
}
