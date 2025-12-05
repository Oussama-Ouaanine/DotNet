using System.Collections.Generic;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Client;

public class ClientBrowseViewModel
{
    public IEnumerable<CategoryBrowseSection> Sections { get; set; } = new List<CategoryBrowseSection>();
}

public class CategoryBrowseSection
{
    public Category Category { get; set; } = new();
    public IEnumerable<Book> Books { get; set; } = new List<Book>();
}
