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
        public async Task<User> GetUserAsync(string identifier)
        {
            var result = _context.Tradesmans.Select(user => user)
                .Where(a => a.Cnpj == identifier).FirstOrDefault();

            if(result == null) { 
                throw new UserException("Não encontrado");
            }

            return result;
        }

        public async Task SaveUserAsync(Tradesman tradesman)
        {
            //Validate

            var result = _context.Tradesmans.Select(user => user)
                .Where(a => a.Cnpj == tradesman.Cnpj || a.Email == tradesman.Email).Any();

            if(result) {
                throw new UserException("Cnpj ou email já cadastrados");
            }

            //Save
            _context.Tradesmans.Add(tradesman);
        }

        public async Task UpdateUserAsync(Tradesman tradesman)
        {
            //Validate

            var result = _context.Tradesmans.Select(user => user)
                .Where(a => a.Cnpj == tradesman.Cnpj || a.Email == tradesman.Email).Any();

            if (result)
            {
                throw new UserException("Cnpj ou email já cadastrados.");
            }

            var oldTradesman = _context.Tradesmans.Select(user => user).Where(a => a.Id == tradesman.Id).Any();
            if (!oldTradesman)
            {
                throw new UserException("Usuário não encontrado.");
            }

            //Save
            _context.Tradesmans.Update(tradesman);
        }

        public async Task DeleteUserAsync(string identifier)
        {
            //Validate
            var result = _context.Tradesmans.Select(user => user)
                 .Where(a => a.Cnpj == identifier).FirstOrDefault();

            if (result == null)
            {
                throw new UserException("Não encontrado");
            }

            //Delete
            _context.Tradesmans.Remove(result);
        }
    }
}
