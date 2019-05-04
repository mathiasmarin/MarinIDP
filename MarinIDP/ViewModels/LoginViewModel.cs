using System.ComponentModel.DataAnnotations;

namespace MarinIDP.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-post måste anges")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postaddress")]
        [Display(Name = "Epost")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lösenord måste anges")]
        [DataType(DataType.Password)]
        [Display(Name = "Lösenord")]
        public string Password { get; set; }
    }
}