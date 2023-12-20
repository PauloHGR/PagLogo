using PagLogo.Enums;

namespace PagLogo.Models
{
    public class UserFilterRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserType UserType { get; set; }
        public string? Identifier { get; set; }
    }
}
