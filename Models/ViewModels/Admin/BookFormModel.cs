using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Admin;

public class BookFormModel
{
    public string? Id { get; set; }

    [Required]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Author { get; set; } = string.Empty;

    [Display(Name = "Category")]
    [Required]
    public string CategoryId { get; set; } = string.Empty;

    [Display(Name = "Description")]
    [DataType(DataType.MultilineText)]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Published year")]
    public int? PublishedYear { get; set; }

    [Display(Name = "Page count")]
    public int? Pages { get; set; }

    public double Rating { get; set; } = 4.5;
    public bool IsAvailable { get; set; } = true;
    public bool IsFeatured { get; set; }
    public string? ExistingCoverPath { get; set; }
    public IEnumerable<Category> Categories { get; set; } = new List<Category>();
}
