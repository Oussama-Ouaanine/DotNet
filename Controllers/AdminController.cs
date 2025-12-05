using System;
using System.IO;
using System.Linq;
using LibraryWebApp.Models;
using LibraryWebApp.Models.ViewModels.Admin;
using LibraryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers;

public class AdminController : Controller
{
	private readonly BookService _bookService;
	private readonly CategoryService _categoryService;
	private readonly BookingService _bookingService;
	private readonly UserService _userService;

	public AdminController(
		BookService bookService,
		CategoryService categoryService,
		BookingService bookingService,
		UserService userService)
	{
		_bookService = bookService;
		_categoryService = categoryService;
		_bookingService = bookingService;
		_userService = userService;
	}

	public IActionResult Index()
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var model = new AdminDashboardViewModel
		{
			TotalBooks = _bookService.GetAll().Count(),
			TotalCategories = _categoryService.GetAll().Count(),
			ActiveBookings = _bookingService.GetAll().Count(b => b.Status == BookingStatus.Pending || b.Status == BookingStatus.Approved),
			TotalMembers = _userService.GetMembers().Count(),
			RecentBookings = _bookingService.GetRecent(5),
			FeaturedBooks = _bookService.GetFeatured(4)
		};

		return View(model);
	}

	public IActionResult Books(string? search, string? category)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var books = _bookService.GetAll();

		if (!string.IsNullOrWhiteSpace(search))
		{
			books = books.Where(b => b.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
								   || b.Author.Contains(search, StringComparison.OrdinalIgnoreCase));
		}

		if (!string.IsNullOrWhiteSpace(category) && !category.Equals("all", StringComparison.OrdinalIgnoreCase))
		{
			books = books.Where(b => b.CategoryId.Equals(category, StringComparison.OrdinalIgnoreCase));
		}

		var model = new AdminBooksViewModel
		{
			Books = books.ToList(),
			Categories = _categoryService.GetAll(),
			SearchTerm = search,
			CategoryFilter = category
		};

		return View(model);
	}

	[HttpGet]
	public IActionResult CreateBook()
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var model = new BookFormModel
		{
			Categories = _categoryService.GetAll(),
			IsAvailable = true
		};

		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult CreateBook(BookFormModel model, IFormFile? coverImage)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		if (!ModelState.IsValid)
		{
			model.Categories = _categoryService.GetAll();
			return View(model);
		}

		var book = MapToBook(model);
		book.CoverImagePath = SaveCoverIfNecessary(coverImage) ?? model.ExistingCoverPath;

		_bookService.Add(book);
		TempData["StatusMessage"] = $"“{book.Title}” added to the collection.";

		return RedirectToAction(nameof(Books));
	}

	[HttpGet]
	public IActionResult EditBook(string id)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var book = _bookService.GetById(id);
		if (book is null)
		{
			return RedirectToAction(nameof(Books));
		}

		var model = MapToFormModel(book);
		model.Categories = _categoryService.GetAll();
		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult EditBook(BookFormModel model, IFormFile? coverImage)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		if (!ModelState.IsValid)
		{
			model.Categories = _categoryService.GetAll();
			return View(model);
		}

		var book = MapToBook(model);
		book.Id = model.Id ?? string.Empty;
		book.CoverImagePath = SaveCoverIfNecessary(coverImage) ?? model.ExistingCoverPath;

		_bookService.Update(book);
		TempData["StatusMessage"] = $"“{book.Title}” updated.";

		return RedirectToAction(nameof(Books));
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult DeleteBook(string id)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		_bookService.Remove(id);
		TempData["StatusMessage"] = "Book removed from catalogue.";
		return RedirectToAction(nameof(Books));
	}

	public IActionResult Categories()
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var categories = _categoryService.GetAll();
		return View(categories);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult CreateCategory(string name, string description, string icon)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		if (string.IsNullOrWhiteSpace(name))
		{
			TempData["StatusMessage"] = "Category name is required.";
			return RedirectToAction(nameof(Categories));
		}

		_categoryService.Add(new Category
		{
			Name = name.Trim(),
			Description = description?.Trim() ?? string.Empty,
			Icon = string.IsNullOrWhiteSpace(icon) ? "◎" : icon.Trim()
		});

		TempData["StatusMessage"] = "Category created.";
		return RedirectToAction(nameof(Categories));
	}

	public IActionResult Clients()
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var clients = _userService.GetMembers();
		return View(clients);
	}

	public IActionResult Bookings()
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var bookings = _bookingService.GetAll();
		return View(bookings);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult ApproveBooking(string id, DateTime dueDate)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		if (dueDate.Date < DateTime.UtcNow.Date)
		{
			TempData["StatusMessage"] = "Due date cannot be in the past.";
			TempData["StatusType"] = "error";
			return RedirectToAction(nameof(Bookings));
		}

		var success = _bookingService.ApproveBooking(id, dueDate.ToUniversalTime());
		if (!success)
		{
			TempData["StatusMessage"] = "Unable to approve this reservation.";
			TempData["StatusType"] = "error";
		}
		else
		{
			TempData["StatusMessage"] = "Reservation approved. The member has been notified.";
			TempData["StatusType"] = "success";
		}

		return RedirectToAction(nameof(Bookings));
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult RefuseBooking(string id)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var success = _bookingService.RefuseBooking(id);
		TempData["StatusMessage"] = success
			? "Reservation request refused."
			: "Unable to refuse this reservation.";
		TempData["StatusType"] = success ? "success" : "error";

		return RedirectToAction(nameof(Bookings));
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult CompleteBooking(string id)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var success = _bookingService.MarkReturned(id);
		TempData["StatusMessage"] = success
			? "Booking marked as returned."
			: "Unable to update booking status.";
		TempData["StatusType"] = success ? "success" : "error";
		return RedirectToAction(nameof(Bookings));
	}

	public IActionResult History(string? status, string? search)
	{
		var gate = EnsureAdmin();
		if (gate is not null) return gate;

		var bookings = _bookingService.GetHistory(status, search);
		var overdueBookings = _bookingService.GetOverdueBookings();

		ViewBag.StatusFilter = status;
		ViewBag.SearchQuery = search;
		ViewBag.OverdueCount = overdueBookings.Count();
		ViewBag.OverdueBookings = overdueBookings;

		return View(bookings);
	}

	private IActionResult? EnsureAdmin()
	{
		var role = HttpContext.Session.GetString("Role");
		if (!string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase))
		{
			return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
		}

		return null;
	}

	private static Book MapToBook(BookFormModel model)
	{
		var book = new Book
		{
			Id = model.Id ?? Guid.NewGuid().ToString("N"),
			Title = model.Title.Trim(),
			Author = model.Author.Trim(),
			CategoryId = model.CategoryId,
			Description = model.Description?.Trim() ?? string.Empty,
			IsAvailable = model.IsAvailable,
			IsFeatured = model.IsFeatured,
			Pages = model.Pages ?? 0,
			Rating = model.Rating,
			PublishedOn = model.PublishedYear.HasValue
				? new DateTime(model.PublishedYear.Value, 1, 1)
				: DateTime.UtcNow
		};

		return book;
	}

	private BookFormModel MapToFormModel(Book book)
	{
		return new BookFormModel
		{
			Id = book.Id,
			Title = book.Title,
			Author = book.Author,
			CategoryId = book.CategoryId,
			Description = book.Description,
			IsAvailable = book.IsAvailable,
			IsFeatured = book.IsFeatured,
			Pages = book.Pages,
			Rating = book.Rating,
			PublishedYear = book.PublishedOn.Year,
			ExistingCoverPath = book.CoverImagePath
		};
	}

	private static string? SaveCoverIfNecessary(IFormFile? coverImage)
	{
		if (coverImage is null || coverImage.Length == 0)
		{
			return null;
		}

		var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "books");
		Directory.CreateDirectory(uploadsDirectory);

		var fileName = $"{Guid.NewGuid():N}{Path.GetExtension(coverImage.FileName)}";
		var filePath = Path.Combine(uploadsDirectory, fileName);

		using var stream = new FileStream(filePath, FileMode.Create);
		coverImage.CopyTo(stream);

		return $"/uploads/books/{fileName}";
	}
}
