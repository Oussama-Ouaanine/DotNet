using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Models;
using LibraryWebApp.Services;

namespace LibraryWebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly BookService _bookService;
        private readonly BookingService _bookingService;
        private readonly UserService _userService;

        public ClientController(BookService bookService, BookingService bookingService, UserService userService)
        {
            _bookService = bookService;
            _bookingService = bookingService;
            _userService = userService;
        }

        private bool IsClient()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Client";
        }

        private int GetUserId()
        {
            return HttpContext.Session.GetInt32("UserIdInt") ?? 0;
        }

        // GET: Client/Index
        public async Task<IActionResult> Index()
        {
            if (!IsClient()) return RedirectToAction("Login", "Account");

            var books = await _bookService.GetAvailableBooksAsync();
            return View(books);
        }

        // GET: Client/Browse
        public async Task<IActionResult> Browse(string category = "")
        {
            if (!IsClient()) return RedirectToAction("Login", "Account");

            List<Book> books;
            if (string.IsNullOrEmpty(category))
            {
                books = await _bookService.GetAllAsync();
            }
            else
            {
                books = await _bookService.GetByCategoryAsync(category);
            }

            return View(books);
        }

        // GET: Client/Search
        public async Task<IActionResult> Search(string searchTerm)
        {
            if (!IsClient()) return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(searchTerm))
            {
                return View(new List<Book>());
            }

            var books = await _bookService.SearchBooksAsync(searchTerm);
            ViewBag.SearchTerm = searchTerm;
            return View(books);
        }

        // GET: Client/BookDetails/5
        public async Task<IActionResult> BookDetails(string id)
        {
            if (!IsClient()) return RedirectToAction("Login", "Account");

            var book = await _bookService.GetByIdAsync(id);
            if (book == null) return NotFound();

            return View(book);
        }

        // POST: Client/BookABook/5
        [HttpPost]
        public async Task<IActionResult> BookABook(string id)
        {
            if (!IsClient()) return RedirectToAction("Login", "Account");

            var book = await _bookService.GetByIdAsync(id);
            if (book == null || book.AvailableCopies <= 0)
            {
                TempData["Error"] = "Book is not available";
                return RedirectToAction("Index");
            }

            var userId = GetUserId();
            var userIdString = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("Username");

            var booking = new Booking
            {
                UserId = userId,
                UserObjectId = userIdString,
                Username = username ?? "",
                BookId = book.BookId,
                BookObjectId = id,
                BookTitle = book.Title,
                BookingDate = DateTime.Now,
                Status = "Pending"
            };

            await _bookingService.CreateAsync(booking);
            TempData["Success"] = "Booking request submitted successfully";
            return RedirectToAction("MyBookings");
        }

        // GET: Client/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            if (!IsClient()) return RedirectToAction("Login", "Account");

            var userId = GetUserId();
            var bookings = await _bookingService.GetByUserIdAsync(userId);
            return View(bookings);
        }

        // POST: Client/CancelBooking/5
        [HttpPost]
        public async Task<IActionResult> CancelBooking(string id)
        {
            if (!IsClient()) return RedirectToAction("Login", "Account");

            var booking = await _bookingService.GetByIdAsync(id);
            if (booking != null && booking.Status == "Pending")
            {
                await _bookingService.DeleteAsync(id);
                TempData["Success"] = "Booking cancelled successfully";
            }

            return RedirectToAction("MyBookings");
        }
    }
}
