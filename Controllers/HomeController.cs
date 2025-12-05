using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Models;
using LibraryWebApp.Models.ViewModels.Home;
using LibraryWebApp.Services;

namespace LibraryWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BookService _bookService;

    public HomeController(ILogger<HomeController> logger, BookService bookService)
    {
        _logger = logger;
        _bookService = bookService;
    }

    public IActionResult Index()
    {
        var model = new HomeLandingViewModel
        {
            FeaturedBooks = _bookService.GetFeatured(9)
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
