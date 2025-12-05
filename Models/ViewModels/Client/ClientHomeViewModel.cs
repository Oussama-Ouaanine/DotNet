using System.Collections.Generic;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Client;

public class ClientHomeViewModel
{
    public IEnumerable<Book> FeaturedBooks { get; set; } = new List<Book>();
    public IEnumerable<Book> TrendingBooks { get; set; } = new List<Book>();
    public IEnumerable<Category> HighlightedCategories { get; set; } = new List<Category>();
}
