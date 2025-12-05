using System.Collections.Generic;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Admin;

public class AdminBooksViewModel
{
    public IEnumerable<Book> Books { get; set; } = new List<Book>();
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    public string? SearchTerm { get; set; }
    public string? CategoryFilter { get; set; }
}
