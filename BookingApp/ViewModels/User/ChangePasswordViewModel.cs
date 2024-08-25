using System.ComponentModel.DataAnnotations;

namespace BookingApp.ViewModels.User
{
	public class ChangePasswordViewModel
	{
		[Required(ErrorMessage = "Old password is required")]
		[DataType(DataType.Password)]
		public string? OldPassword { get; set; }
		[Required(ErrorMessage = "New password is required")]
		[DataType(DataType.Password)]
		public string? NewPassword { get; set; }
		[Required(ErrorMessage = "Confirm Password is required")]
		[DataType(DataType.Password)]
		[Compare("NewPassword", ErrorMessage = "Passwords do not match")]
		public string? ConfirmPassword { get; set; }

	}
}
