using PagLogo.Models;

namespace PagLogo.Services
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string identifier);
        Task SaveUserAsync(Tradesman tradesman);
        Task UpdateUserAsync(Tradesman tradesman);
        Task DeleteUserAsync(string identifier);
    }
}
