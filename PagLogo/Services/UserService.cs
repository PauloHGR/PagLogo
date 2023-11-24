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

        private User GetTradesmanByIdentifier(string identifier)
        {
            var result = _context.Users.Select(user => user)
                .Where(a => a.Identifier == identifier).FirstOrDefault();

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

        public async Task<List<User>> GetAllUsers()
        {
            return _context.Users.Select(x => x).ToList();
        }

        public async Task SaveUserAsync(User user)
        {
            //Validate

            var result = _context.Users.Select(user => user)
                .Where(a => a.Identifier == user.Identifier || a.Email == user.Email).Any();

            if(result) {
                throw new UserException("Usuário ou email já cadastrados");
            }

            //Save
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public async Task UpdateUserAsync(User request)
        {
            //Validate

            var oldUser = GetTradesmanByIdentifier(request.Identifier); ;

            var result = _context.Users.Select(user => user)
               .Where(a => a.Email == request.Email
               && a.Id != oldUser.Id).Any();

            if (result)
            {
                throw new UserException("Usuário ou email já cadastrados.");
            }

            oldUser.Name = request.Name;
            oldUser.Email = request.Email;
            oldUser.Balance = request.Balance;
            oldUser.Password = request.Password;

            //Update
            _context.Users.Update(oldUser);
            _context.SaveChanges();
        }

        public async Task DeleteUserAsync(string identifier)
        {
            //Operation
            var result = GetTradesmanByIdentifier(identifier);

            //Delete
            _context.Users.Remove(result);
            _context.SaveChanges();
        }
    }
}
