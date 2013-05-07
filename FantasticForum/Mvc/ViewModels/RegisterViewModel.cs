using System.ComponentModel.DataAnnotations;

namespace Mvc.ViewModels
{
    public class RegisterViewModel
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(30, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } 
    }
}