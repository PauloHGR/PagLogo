using PagLogo.Models;

namespace PagLogo.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string identifier);
        Task SaveUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string identifier);
    }
}
