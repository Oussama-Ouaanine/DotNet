using System;

namespace LibraryWebApp.Models;

public class Book
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Title { get; set; } = string.Empty;
	public string Author { get; set; } = string.Empty;
	public string CategoryId { get; set; } = string.Empty;
	public string CategoryName { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public string? CoverImagePath { get; set; }
	public bool IsAvailable { get; set; } = true;
	public DateTime PublishedOn { get; set; } = DateTime.UtcNow;
	public double Rating { get; set; } = 4.5;
	public int Pages { get; set; } = 320;
	public bool IsFeatured { get; set; }
}
