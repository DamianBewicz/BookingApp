using Microsoft.AspNetCore.Mvc;
using BookingApp.Data;
using Microsoft.AspNetCore.Identity;
using BookingApp.Models;
using BookingApp.ViewModels.User;

namespace BookingApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            User? user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);

            if (user != null) 
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            User newUser = new User()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.UserName,
            };

            var response = await _userManager.CreateAsync(newUser, registerViewModel.Password);

   //         if (response.Succeeded)
   //         {
   //             Console.WriteLine("SUCCESS");
   //         }
   //         else 
   //         {
			//	Console.WriteLine("FAILED");
			//	Console.WriteLine(response.Errors);
			//	Console.WriteLine(response.Errors.ToList());
			//	Console.WriteLine(response.Errors.First());
			//	var message = string.Join(", ", response.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));
   //             Console.WriteLine(message);
			//}

			return View();
		}
	}
}
