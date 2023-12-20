using PagLogo.Models;
using System.IdentityModel.Tokens.Jwt;

namespace PagLogo.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers(UserFilterRequest request);
        Task<User> GetUserAsync(string identifier);
        Task SaveUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string identifier);

        Task<JwtSecurityToken> AuthenticateUser(LoginRequest loginRequest);
    }
}
