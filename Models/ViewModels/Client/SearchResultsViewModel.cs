using System.Collections.Generic;
using System.Linq;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Client;

public class SearchResultsViewModel
{
    public string Query { get; set; } = string.Empty;
    public IEnumerable<Book> Results { get; set; } = new List<Book>();
    public bool HasResults => Results != null && Results.Any();
}
