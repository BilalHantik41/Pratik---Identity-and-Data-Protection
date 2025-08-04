using System.ComponentModel.DataAnnotations;

namespace Pratik___Identity_and_Data_Protection.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public  string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
