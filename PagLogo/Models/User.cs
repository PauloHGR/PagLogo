using PagLogo.Enums;
using System.Data.Common;

namespace PagLogo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public double? Balance { get; set; }
        public UserType UserType { get; set; }
        public string? Identifier { get; set; }

    }
}
