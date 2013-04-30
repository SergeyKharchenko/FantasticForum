using System.ComponentModel.DataAnnotations;

namespace Mvc.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}