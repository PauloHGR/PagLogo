using PagLogo.Exceptions;
using PagLogo.Models;

namespace PagLogo.Services
{
    public class UserService : IUserService
    {
        private readonly IAppDbContext _context;
        public UserService(IAppDbContext context) { 
            _context = context;
        }
        public async Task<User> GetUsersAsync(string identifier)
        {
            var result = _context.Tradesmans.Select(user => user)
                .Where(a => a.Cnpj == identifier).FirstOrDefault();

            if(result == null) { 
                throw new UserException("Não encontrado");
            }

            return result;
        }
    }
}
