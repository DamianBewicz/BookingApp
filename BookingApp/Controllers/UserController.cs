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
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
		}


		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(!ModelState.IsValid) { return View(model); }

            User? user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
                if (isPasswordCorrect)
                {
                    Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return View(model);
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

            if (response.Succeeded)
            {
				return RedirectToAction("Index", "Home");
			}

			foreach (var error in response.Errors)
			{
				ModelState.AddModelError("Password", error.Description);
			}

			return View(registerViewModel);
		}
	}
}
