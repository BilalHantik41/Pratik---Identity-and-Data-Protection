using System.ComponentModel.DataAnnotations;

namespace Pratik___Identity_and_Data_Protection.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
