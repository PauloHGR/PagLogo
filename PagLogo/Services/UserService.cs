using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PagLogo.Exceptions;
using PagLogo.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        private bool isValidRequestFilter(UserFilterRequest request)
        {

            if(!string.IsNullOrEmpty(request.Name) || 
                !string.IsNullOrEmpty(request.Email) || 
                !string.IsNullOrEmpty(request.Identifier) || 
                !(request.UserType == 0))
                return true;

            return false;
        }

        private IQueryable<User> ConfigureSortingAndPagination(IQueryable<User> query,UserFilterRequest request)
        {
            Expression<Func<User, object>> keySelector = request.SortField.ToString() switch
            {
                "Name" => user => user.Name,
                "Email" => user => user.Email,
                "Balance" => user => user.Balance,
                "Identifier" => user => user.Identifier,
                "UserType" => user => user.UserType,
                _ => user => user.Id,
            };
            query = (request.SortOrder == Enums.SortOrder.DESC) ? query.OrderByDescending(keySelector) : 
                query.OrderBy(keySelector);
            query = query.Skip(request.Offset).Take(request.Size);

            return query;
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsers(UserFilterRequest request)
        {
            IQueryable<User> _userContext = _context.Users;
            var query = _userContext;

            if (isValidRequestFilter(request))
            {
                query = query
                    .Select(user => user)
                    .Where(a => a.Name.Contains(request.Name) ||
                        a.Identifier.Contains(request.Identifier) ||
                        (a.UserType == request.UserType) ||
                        a.Email.Contains(request.Email));
            }

            query = ConfigureSortingAndPagination(query, request);

            var result = query.Select(user => new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Balance = user.Balance,
                UserType = user.UserType,
                Identifier = user.Identifier,

            }).ToList();

            return result;
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
