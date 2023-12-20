using System.ComponentModel.DataAnnotations;

namespace PagLogo.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage ="Identifier is required")]
        public string? Identifier { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
    }
}
