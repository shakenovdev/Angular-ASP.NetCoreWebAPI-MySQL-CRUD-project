using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    public class SignInViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}