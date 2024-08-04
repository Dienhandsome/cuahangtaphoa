﻿using CUAHANG_TAPHOA.Models;
using CUAHANG_TAPHOA.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CUAHANG_TAPHOA.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<AppUserModel> _userManage;
		private SignInManager<AppUserModel> _signInManager;
		public AccountController(SignInManager<AppUserModel> signInManager, UserManager<AppUserModel> userManager)
		{
			_userManage = userManager;
			_signInManager = signInManager;
		}

		
		public IActionResult Login(string returnUrl)
		{
			return View(new LoginViewModel { ReturnUrl = returnUrl });
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if(ModelState.IsValid)
			{
				Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginVM.UserName, loginVM.Password, false, false);
				if (result.Succeeded)
				{
					return Redirect(loginVM.ReturnUrl ?? "/");  // TRẢ VỀ TRANG HOME
				}
				ModelState.AddModelError("", "Invalid UserName à PassWord");
			}
			return View(loginVM);
		}


		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		
		public  async Task<IActionResult> Create(UserModel user)
		{
			if(ModelState.IsValid)
			{
				AppUserModel newUser = new AppUserModel { UserName = user.UserName, Email = user.Email};
			    IdentityResult result = await _userManage.CreateAsync(newUser, user.Password);
				if(result.Succeeded)
				{
					TempData["success"] = "Tạo user thành công";
					return Redirect("/account/Login");
				}
                else
                {
					foreach(IdentityError error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}
                }
            }
			return View(user);
		}

		public async Task<IActionResult> Logout(string returnUrl = "/")
		{
			await _signInManager.SignOutAsync(); // thoats ra khoi login
			return Redirect(returnUrl);
		}
	}
}
