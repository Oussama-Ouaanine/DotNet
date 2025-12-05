using LibraryWebApp.Models.ViewModels.Account;
using LibraryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers;

public class AccountController : Controller
{
	private readonly UserService _userService;

	public AccountController(UserService userService)
	{
		_userService = userService;
	}

	[HttpGet]
	public IActionResult Login(string? returnUrl = null)
	{
		ViewData["ReturnUrl"] = returnUrl;
		return View(new LoginInputModel());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Login(LoginInputModel model, string? returnUrl = null)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		var user = _userService.ValidateCredentials(model.Email, model.Password);
		if (user is null)
		{
			ModelState.AddModelError(string.Empty, "Invalid email or password.");
			return View(model);
		}

		HttpContext.Session.SetString("Username", user.FullName);
		HttpContext.Session.SetString("Email", user.Email);
		HttpContext.Session.SetString("Role", user.IsAdmin ? "Admin" : "Client");
		HttpContext.Session.SetString("UserId", user.Id);

		if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
		{
			return Redirect(returnUrl);
		}

		return user.IsAdmin
			? RedirectToAction("Index", "Admin")
			: RedirectToAction("Index", "Client");
	}

	[HttpGet]
	public IActionResult Register()
	{
		return View(new RegisterInputModel());
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Register(RegisterInputModel model)
	{
		if (!ModelState.IsValid)
		{
			return View(model);
		}

		try
		{
			var user = _userService.Register(model);

			HttpContext.Session.SetString("Username", user.FullName);
			HttpContext.Session.SetString("Email", user.Email);
			HttpContext.Session.SetString("Role", "Client");
			HttpContext.Session.SetString("UserId", user.Id);

			return RedirectToAction("Index", "Client");
		}
		catch (Exception ex)
		{
			ModelState.AddModelError(string.Empty, ex.Message);
			return View(model);
		}
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Logout()
	{
		HttpContext.Session.Clear();
		return RedirectToAction("Index", "Home");
	}
}
