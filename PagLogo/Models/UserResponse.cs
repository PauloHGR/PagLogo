using PagLogo.Enums;

namespace PagLogo.Models
{
    public class UserResponse
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public double? Balance { get; set; }
        public UserType UserType { get; set; }
        public string? Identifier { get; set; }

        public string GetExpression(SortOrder SortOrder, UserSortField SortField)
        {
            return SortOrder.ToString() + " " + SortField.ToString();
        }
    }
}
