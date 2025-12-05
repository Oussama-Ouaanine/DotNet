using System;
using System.Collections.Generic;
using System.Linq;
using LibraryWebApp.Models;

namespace LibraryWebApp.Services;

public class CategoryService
{
	private readonly List<Category> _categories;
	private readonly object _mutex = new();

	public CategoryService()
	{
		_categories = new List<Category>
		{
			new() { Id = "fiction", Name = "Contemporary Fiction", Description = "Stories to spark imagination.", Icon = "✦" },
			new() { Id = "technology", Name = "Technology & Innovation", Description = "Stay ahead of the curve.", Icon = "⌁" },
			new() { Id = "business", Name = "Business & Leadership", Description = "Strategies from the world's best minds.", Icon = "◎" },
			new() { Id = "wellbeing", Name = "Wellbeing", Description = "Mindfulness, wellness, and better living.", Icon = "☼" }
		};
	}

	public IEnumerable<Category> GetAll() => _categories;

	public Category? GetById(string id) => _categories.FirstOrDefault(c => c.Id == id);

	public void Add(Category category)
	{
		lock (_mutex)
		{
			if (string.IsNullOrWhiteSpace(category.Id))
			{
				category.Id = Guid.NewGuid().ToString("N");
			}

			_categories.Add(category);
		}
	}
}
