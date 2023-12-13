using Microsoft.IdentityModel.Tokens;
using PagLogo.Exceptions;
using PagLogo.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PagLogo.Services
{
    public class UserService : IUserService
    {
        private readonly IAppDbContext _context;
        private readonly IConfiguration _configuration;
        public UserService(IAppDbContext context, IConfiguration configuration) { 
            _context = context;
            _configuration = configuration;
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

        private bool CheckPassword(string userPassword, string requestPassword)
        {
            return string.Equals(userPassword, requestPassword);
        }

        public async Task<JwtSecurityToken> AuthenticateUser(LoginRequest loginRequest)
        {
            var user = GetTradesmanByIdentifier(loginRequest.Identifier);

            if(user != null && CheckPassword(user.Password, loginRequest.Password)) {

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Identifier),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "User")
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(12),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                return token;

            } else
            {
                throw new AuthenticateException("User not existent or password incorrect");
            }
        }
    }
}
