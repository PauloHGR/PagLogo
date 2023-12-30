using PagLogo.Enums;

namespace PagLogo.Models
{
    public class UserFilterRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public UserType UserType { get; set; }
        public string? Identifier { get; set; }
        public SortOrder SortOrder { get; set; }
        public UserSortField SortField { get; set; }
        public int Size { get; set; } = 20;
        public int Offset { get; set; } = 0;

    }
}
