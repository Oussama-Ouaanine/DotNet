using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibraryWebApp.Models;

namespace LibraryWebApp.Services;

public class BookService
{
	private readonly List<Book> _books;
	private readonly CategoryService _categoryService;
	private readonly object _mutex = new();

	public BookService(CategoryService categoryService)
	{
		_categoryService = categoryService;
		_books = SeedBooks();
	}

	public IEnumerable<Book> GetAll() => _books.OrderByDescending(b => b.IsFeatured).ThenBy(b => b.Title);

	public IEnumerable<Book> GetFeatured(int count = 6) => _books.Where(b => b.IsFeatured).Take(count);

	public IEnumerable<Book> GetByCategory(string categoryId) =>
		_books.Where(b => b.CategoryId.Equals(categoryId, StringComparison.OrdinalIgnoreCase));

	public IEnumerable<Book> Search(string term)
	{
		if (string.IsNullOrWhiteSpace(term)) return Enumerable.Empty<Book>();

		term = term.Trim().ToLowerInvariant();
		return _books.Where(b => b.Title.ToLowerInvariant().Contains(term)
							  || b.Author.ToLowerInvariant().Contains(term)
							  || b.Description.ToLowerInvariant().Contains(term));
	}

	public Book? GetById(string id) => _books.FirstOrDefault(b => b.Id == id);

	public void Add(Book book)
	{
		lock (_mutex)
		{
			book.Id = Guid.NewGuid().ToString("N");
			book.CategoryName = _categoryService.GetById(book.CategoryId)?.Name ?? book.CategoryName;
			_books.Add(book);
		}
	}

	public void Update(Book book)
	{
		lock (_mutex)
		{
			var existing = _books.FirstOrDefault(b => b.Id == book.Id);
			if (existing is null) return;

			existing.Title = book.Title;
			existing.Author = book.Author;
			existing.Description = book.Description;
			existing.CategoryId = book.CategoryId;
			existing.CategoryName = _categoryService.GetById(book.CategoryId)?.Name ?? existing.CategoryName;
			existing.IsAvailable = book.IsAvailable;
			existing.IsFeatured = book.IsFeatured;
			existing.Pages = book.Pages;
			existing.PublishedOn = book.PublishedOn;
			existing.Rating = book.Rating;
			if (!string.IsNullOrWhiteSpace(book.CoverImagePath))
			{
				existing.CoverImagePath = book.CoverImagePath;
			}
		}
	}

	public void Remove(string id)
	{
		lock (_mutex)
		{
			var book = _books.FirstOrDefault(b => b.Id == id);
			if (book is null) return;

			_books.Remove(book);

			if (!string.IsNullOrWhiteSpace(book.CoverImagePath))
			{
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", book.CoverImagePath.TrimStart('/'));
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}
	}

	public void MarkReserved(string bookId)
	{
		lock (_mutex)
		{
			var book = _books.FirstOrDefault(b => b.Id == bookId);
			if (book is null) return;
			book.IsAvailable = false;
		}
	}

	public void MarkReturned(string bookId)
	{
		lock (_mutex)
		{
			var book = _books.FirstOrDefault(b => b.Id == bookId);
			if (book is null) return;
			book.IsAvailable = true;
		}
	}

	private List<Book> SeedBooks()
	{
		var categories = _categoryService.GetAll().ToDictionary(c => c.Id, c => c.Name);

		var books = new List<Book>
		{
			new()
			{
				Id = "deep-work",
				Title = "Deep Work",
				Author = "Cal Newport",
				CategoryId = "business",
				CategoryName = categories.GetValueOrDefault("business", "Business"),
				Description = "Rules for focused success in a distracted world.",
				CoverImagePath = "https://images.unsplash.com/photo-1528207776546-365bb710ee93?auto=format&fit=crop&w=600&q=80",
				IsFeatured = true,
				Rating = 4.7,
				Pages = 304,
				PublishedOn = new DateTime(2016, 1, 5)
			},
			new()
			{
				Id = "atomic-habits",
				Title = "Atomic Habits",
				Author = "James Clear",
				CategoryId = "wellbeing",
				CategoryName = categories.GetValueOrDefault("wellbeing", "Wellbeing"),
				Description = "An easy & proven way to build good habits & break bad ones.",
				CoverImagePath = "https://images.unsplash.com/photo-1512820790803-83ca734da794?auto=format&fit=crop&w=600&q=80",
				IsFeatured = true,
				Rating = 4.9,
				Pages = 320,
				PublishedOn = new DateTime(2018, 10, 16)
			},
			new()
			{
				Id = "clean-code",
				Title = "Clean Code",
				Author = "Robert C. Martin",
				CategoryId = "technology",
				CategoryName = categories.GetValueOrDefault("technology", "Technology"),
				Description = "A handbook of agile software craftsmanship.",
				CoverImagePath = "https://images.unsplash.com/photo-1544947950-fa07a98d237f?auto=format&fit=crop&w=600&q=80",
				IsFeatured = true,
				Rating = 4.8,
				Pages = 464,
				PublishedOn = new DateTime(2008, 8, 11)
			},
			new()
			{
				Id = "project-hail-mary",
				Title = "Project Hail Mary",
				Author = "Andy Weir",
				CategoryId = "fiction",
				CategoryName = categories.GetValueOrDefault("fiction", "Fiction"),
				Description = "A lone astronaut must save the earth from disaster.",
				CoverImagePath = "https://images.unsplash.com/photo-1495446815901-a7297e633e8d?auto=format&fit=crop&w=600&q=80",
				Rating = 4.6,
				Pages = 496,
				PublishedOn = new DateTime(2021, 5, 4)
			},
			new()
			{
				Id = "range",
				Title = "Range",
				Author = "David Epstein",
				CategoryId = "business",
				CategoryName = categories.GetValueOrDefault("business", "Business"),
				Description = "Why generalists triumph in a specialized world.",
				CoverImagePath = "https://images.unsplash.com/photo-1544716278-ca5e3f4abd8c?auto=format&fit=crop&w=600&q=80",
				Rating = 4.5,
				Pages = 352,
				PublishedOn = new DateTime(2019, 5, 28)
			},
			new()
			{
				Id = "flow",
				Title = "Flow",
				Author = "Mihaly Csikszentmihalyi",
				CategoryId = "wellbeing",
				CategoryName = categories.GetValueOrDefault("wellbeing", "Wellbeing"),
				Description = "The psychology of optimal experience.",
				CoverImagePath = "https://images.unsplash.com/photo-1516972810927-80185027ca84?auto=format&fit=crop&w=600&q=80",
				Rating = 4.4,
				Pages = 336,
				PublishedOn = new DateTime(1990, 1, 1)
			}
		};

		return books;
	}
}
