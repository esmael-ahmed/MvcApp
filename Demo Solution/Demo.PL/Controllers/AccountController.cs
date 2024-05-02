using Demo.DAL.Models;
using Demo.PL.Helpers;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
		// Register
		#region Register
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new ApplicationUser()
				{
					UserName = model.Email.Split('@')[0],
					FName = model.FName,
					Email = model.Email,
					LName = model.LName,
					IsAgree = model.IsAgree
				};
				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					return RedirectToAction(nameof(Login));
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}

			}
			return View(model);

		}
		#endregion


		#region Login
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var result = await _userManager.CheckPasswordAsync(user, model.Password);
					if (result)
					{
						var sigInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						if (sigInResult.Succeeded)
						{
							return RedirectToAction("Index", "Home");
						}
						
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Password Is InCorrect");
					}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email Is Not Exsits");
				}
			}
			return View(model);
		}
		#endregion

		#region SignOut
		public new async Task<IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
		#endregion

		#region ForgetPassword
		public IActionResult ForgetPassword()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{

				
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);
					var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = model.Email, Token = token }, Request.Scheme);
					// send email
					var email = new Email()
					{
						To = model.Email,
						Subject = "Reset Password",
						Body = ResetPasswordLink
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction(nameof(CheckYourInbox));
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email Is Not Exists");
				}
			}
			return View("ForgetPassword", model);
		}

		public IActionResult CheckYourInbox()
		{
			return View();
		}
		#endregion

		#region ResetPassword
		public IActionResult ResetPassword(string email, string Token)
		{
			TempData["email"] = email;
			TempData["token"] = Token;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				string email = TempData["email"] as string;
				string token = TempData["token"] as string;
				var user = await _userManager.FindByEmailAsync(email);
				var result =  await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

                if (result.Succeeded)
                {
					return RedirectToAction(nameof(Login));
                }
				else
				{
                    foreach (var error in result.Errors)
                    {
						ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
			return View(model);
		}


		#endregion

	}
}
