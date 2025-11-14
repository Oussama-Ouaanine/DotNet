using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Models;
using LibraryWebApp.Services;

namespace LibraryWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        // GET: Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and password are required";
                return View();
            }

            var user = await _userService.AuthenticateAsync(username, password);
            
            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserId", user.Id!);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("UserIdInt", user.UserId);

            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Client");
            }
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            // Check if username already exists
            var existingUser = await _userService.GetByUsernameAsync(user.Username);
            if (existingUser != null)
            {
                ViewBag.Error = "Username already exists";
                return View(user);
            }

            // Check if email already exists
            var existingEmail = await _userService.GetByEmailAsync(user.Email);
            if (existingEmail != null)
            {
                ViewBag.Error = "Email already exists";
                return View(user);
            }

            user.Role = "Client"; // Default role
            user.RegistrationDate = DateTime.Now;

            await _userService.CreateAsync(user);

            ViewBag.Success = "Registration successful! Please login.";
            return RedirectToAction("Login");
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
