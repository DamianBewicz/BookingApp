using System.ComponentModel.DataAnnotations;

namespace BookingApp.ViewModels.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "This field must be a valid email address")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [MinLengthAttribute(6, ErrorMessage = "Username must contain at least 6 characters")]
        public string UserName { get; set; }

    }
}
