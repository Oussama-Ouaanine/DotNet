using System;
using System.Linq;
using LibraryWebApp.Models;
using LibraryWebApp.Models.ViewModels.Client;
using LibraryWebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApp.Controllers;

public class ClientController : Controller
{
	private readonly BookService _bookService;
	private readonly CategoryService _categoryService;
	private readonly BookingService _bookingService;
	private readonly UserService _userService;

	public ClientController(
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
		var model = new ClientHomeViewModel
		{
			FeaturedBooks = _bookService.GetFeatured(6),
			TrendingBooks = _bookService.GetAll().Take(6),
			HighlightedCategories = _categoryService.GetAll().Take(3)
		};

		return View(model);
	}

	public IActionResult Browse()
	{
		var sections = _categoryService.GetAll()
			.Select(category => new CategoryBrowseSection
			{
				Category = category,
				Books = _bookService.GetByCategory(category.Id).Take(6)
			});

		var model = new ClientBrowseViewModel { Sections = sections };
		return View(model);
	}

	public IActionResult BookDetails(string id)
	{
		var book = _bookService.GetById(id);
		if (book is null)
		{
			return RedirectToAction(nameof(Browse));
		}

		var related = _bookService
			.GetByCategory(book.CategoryId)
			.Where(b => b.Id != book.Id)
			.Take(4);

		var model = new BookDetailsViewModel
		{
			Book = book,
			RelatedBooks = related
		};

		return View(model);
	}

	[HttpGet]
	public IActionResult Search(string? query)
	{
		var results = string.IsNullOrWhiteSpace(query)
			? Enumerable.Empty<Models.Book>()
			: _bookService.Search(query);

		var model = new SearchResultsViewModel
		{
			Query = query ?? string.Empty,
			Results = results.ToList()
		};

		return View(model);
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Reserve(string id)
	{
		var gate = EnsureSignedIn();
		if (gate is not null) return gate;

		var userId = HttpContext.Session.GetString("UserId")!;
		var user = _userService.GetById(userId);
		var book = _bookService.GetById(id);

		if (user is null || book is null)
		{
			TempData["StatusMessage"] = "Unable to reserve this title right now.";
			return RedirectToAction(nameof(Browse));
		}

		try
		{
			_bookingService.ReserveBook(user, book);
			TempData["StatusMessage"] = "Request submitted. An administrator will review your reservation.";
			TempData["StatusType"] = "success";
		}
		catch (InvalidOperationException ex)
		{
			TempData["StatusMessage"] = ex.Message;
			TempData["StatusType"] = "error";
			return RedirectToAction("BookDetails", new { id });
		}

		return RedirectToAction(nameof(MyBookings));
	}

	public IActionResult MyBookings()
	{
		var gate = EnsureSignedIn();
		if (gate is not null) return gate;

		var userId = HttpContext.Session.GetString("UserId")!;
		var bookings = _bookingService.GetForUser(userId);

		var model = new MyBookingsViewModel
		{
			Pending = bookings.Where(b => b.Status == BookingStatus.Pending),
			Approved = bookings.Where(b => b.Status == BookingStatus.Approved),
			History = bookings.Where(b => b.Status == BookingStatus.Completed || b.Status == BookingStatus.Refused)
		};

		return View(model);
	}

	private IActionResult? EnsureSignedIn()
	{
		var username = HttpContext.Session.GetString("Username");
		if (string.IsNullOrWhiteSpace(username))
		{
			return RedirectToAction("Login", "Account", new { returnUrl = Request.Path });
		}

		return null;
	}
}
