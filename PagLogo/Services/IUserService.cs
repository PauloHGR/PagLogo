using PagLogo.Models;

namespace PagLogo.Services
{
    public interface IUserService
    {
        Task<User> GetUsersAsync(string identifier);
    }
}
