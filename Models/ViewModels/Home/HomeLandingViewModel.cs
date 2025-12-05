using System.Collections.Generic;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Home;

public class HomeLandingViewModel
{
    public IEnumerable<Book> FeaturedBooks { get; set; } = new List<Book>();
}
