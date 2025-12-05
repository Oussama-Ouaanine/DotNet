using System.Collections.Generic;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Client;

public class BookDetailsViewModel
{
    public Book Book { get; set; } = new();
    public IEnumerable<Book> RelatedBooks { get; set; } = new List<Book>();
}
