using Microsoft.AspNetCore.Mvc;
using BookingApp.Data;
using Microsoft.AspNetCore.Identity;
using BookingApp.Models;
using BookingApp.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

		[AllowAnonymous]
		public IActionResult Login()
		{
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}

        [HttpPost]
		[AllowAnonymous]
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

			ModelState.AddModelError("", "Invalid login attempt.");
			return View(model);
		}

		public IActionResult Register()
        {
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index", "Home");
			}
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

		public IActionResult ChangePassword()
		{
            return View();
		}

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            ClaimsPrincipal currentUser = this.User;
            string? id = _userManager.GetUserId(User);
            User? user = await _userManager.FindByIdAsync(id ?? "");

            if (user != null)
            {
                bool isSameAsOldPassword = await _userManager.CheckPasswordAsync(user, model.NewPassword);
                if (isSameAsOldPassword)
                {
                    ModelState.AddModelError("NewPassword", "New password can't be the same as the old password");
                    return View(model);
                }

                IdentityResult result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    TempData["SuccessMessage"] = "Your password has been succesfully changed";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                return NotFound("Nie znaleziono użytkownika.");
            }
            return View();
        }
    }
}
