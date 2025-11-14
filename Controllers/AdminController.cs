using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Models;
using LibraryWebApp.Services;

namespace LibraryWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookService _bookService;
        private readonly CategoryService _categoryService;
        private readonly BookingService _bookingService;
        private readonly UserService _userService;

        public AdminController(BookService bookService, CategoryService categoryService, 
            BookingService bookingService, UserService userService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _bookingService = bookingService;
            _userService = userService;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }

        // GET: Admin/Index
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            ViewBag.TotalBooks = (await _bookService.GetAllAsync()).Count;
            ViewBag.TotalUsers = (await _userService.GetClientsAsync()).Count;
            ViewBag.PendingBookings = (await _bookingService.GetPendingBookingsAsync()).Count;
            ViewBag.ActiveBookings = (await _bookingService.GetActiveBookingsAsync()).Count;

            return View();
        }

        // GET: Admin/Books
        public async Task<IActionResult> Books()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var books = await _bookService.GetAllAsync();
            return View(books);
        }

        // GET: Admin/CreateBook
        public async Task<IActionResult> CreateBook()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View();
        }

        // POST: Admin/CreateBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(Book book, IFormFile? coverImage)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(book);
            }

            // Handle image upload
            if (coverImage != null && coverImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "books");
                Directory.CreateDirectory(uploadsFolder); // Create folder if not exists

                var uniqueFileName = $"{Guid.NewGuid()}_{coverImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(fileStream);
                }

                book.ImagePath = $"/uploads/books/{uniqueFileName}";
            }

            await _bookService.CreateAsync(book);
            return RedirectToAction("Books");
        }

        // GET: Admin/EditBook/5
        public async Task<IActionResult> EditBook(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var book = await _bookService.GetByIdAsync(id);
            if (book == null) return NotFound();

            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View(book);
        }

        // POST: Admin/EditBook/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBook(string id, Book book, IFormFile? coverImage)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(book);
            }

            // Handle new image upload
            if (coverImage != null && coverImage.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(book.ImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Upload new image
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "books");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = $"{Guid.NewGuid()}_{coverImage.FileName}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await coverImage.CopyToAsync(fileStream);
                }

                book.ImagePath = $"/uploads/books/{uniqueFileName}";
            }

            book.Id = id;
            await _bookService.UpdateAsync(id, book);
            return RedirectToAction("Books");
        }

        // POST: Admin/DeleteBook/5
        [HttpPost]
        public async Task<IActionResult> DeleteBook(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            await _bookService.DeleteAsync(id);
            return RedirectToAction("Books");
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Categories()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var categories = await _categoryService.GetAllAsync();
            return View(categories);
        }

        // GET: Admin/CreateCategory
        public IActionResult CreateCategory()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            return View();
        }

        // POST: Admin/CreateCategory
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryService.CreateAsync(category);
            return RedirectToAction("Categories");
        }

        // GET: Admin/EditCategory/5
        public async Task<IActionResult> EditCategory(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(category);
        }

        // POST: Admin/EditCategory/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(string id, Category category)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                return View(category);
            }

            category.Id = id;
            await _categoryService.UpdateAsync(id, category);
            return RedirectToAction("Categories");
        }

        // POST: Admin/DeleteCategory/5
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            await _categoryService.DeleteAsync(id);
            return RedirectToAction("Categories");
        }

        // GET: Admin/Bookings
        public async Task<IActionResult> Bookings()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var bookings = await _bookingService.GetAllAsync();
            return View(bookings);
        }

        // POST: Admin/ApproveBooking/5
        [HttpPost]
        public async Task<IActionResult> ApproveBooking(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            await _bookingService.ApproveBookingAsync(id);
            return RedirectToAction("Bookings");
        }

        // POST: Admin/DeclineBooking/5
        [HttpPost]
        public async Task<IActionResult> DeclineBooking(string id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            await _bookingService.DeclineBookingAsync(id);
            return RedirectToAction("Bookings");
        }

        // GET: Admin/Clients
        public async Task<IActionResult> Clients()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var clients = await _userService.GetClientsAsync();
            return View(clients);
        }
    }
}
