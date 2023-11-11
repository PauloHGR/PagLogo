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

        private Tradesman GetTradesmanByIdentifier(string identifier)
        {
            var result = _context.Tradesmans.Select(user => user)
                .Where(a => a.Cnpj == identifier).FirstOrDefault();

            if (result == null)
            {
                throw new UserException("Usuário não encontrado.");
            }

            return result;
        }

        public async Task<User> GetUserAsync(string identifier)
        {
            return GetTradesmanByIdentifier(identifier);
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
            _context.SaveChanges();
        }

        public async Task UpdateUserAsync(Tradesman request)
        {
            //Validate

            var oldTradesman = GetTradesmanByIdentifier(request.Cnpj); ;

            var result = _context.Tradesmans.Select(user => user)
               .Where(a => (a.Cnpj == request.Cnpj || a.Email == request.Email)
               && a.Id != oldTradesman.Id).Any();

            if (result)
            {
                throw new UserException("Cnpj ou email já cadastrados.");
            }

            var tradesmanUpdated = new Tradesman
            {
                Cnpj = oldTradesman.Cnpj,
                Email = oldTradesman.Email,
                Name = oldTradesman.Name,
                Balance = oldTradesman.Balance,
                Password = oldTradesman.Password,
            };

            //Update
            _context.Tradesmans.Update(tradesmanUpdated);
            _context.SaveChanges();
        }

        public async Task DeleteUserAsync(string identifier)
        {
            //Operation
            var result = GetTradesmanByIdentifier(identifier);

            //Delete
            _context.Tradesmans.Remove(result);
            _context.SaveChanges();
        }
    }
}
