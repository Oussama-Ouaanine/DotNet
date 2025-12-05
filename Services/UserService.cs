using System;
using System.Collections.Generic;
using System.Linq;
using LibraryWebApp.Models;
using LibraryWebApp.Models.ViewModels.Account;

namespace LibraryWebApp.Services;

public class UserService
{
	private readonly List<User> _users = new();
	private readonly object _mutex = new();

	public UserService()
	{
		SeedUsers();
	}

	public IEnumerable<User> GetAll() => _users;

	public IEnumerable<User> GetMembers() => _users.Where(u => !u.IsAdmin);

	public User? GetById(string id) => _users.FirstOrDefault(u => u.Id == id);

	public User? GetByEmail(string email) =>
		_users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

	public User? ValidateCredentials(string email, string password)
	{
		var user = GetByEmail(email);
		if (user is null) return null;
		return user.Password == password ? user : null;
	}

	public User Register(RegisterInputModel input, bool isAdmin = false)
	{
		lock (_mutex)
		{
			var existing = GetByEmail(input.Email);
			if (existing is not null)
			{
				throw new InvalidOperationException("An account with this email already exists.");
			}

			var user = new User
			{
				Id = Guid.NewGuid().ToString("N"),
				Email = input.Email,
				Username = input.Email,
				Password = input.Password,
				FullName = input.FullName,
				IsAdmin = isAdmin
			};

			_users.Add(user);
			return user;
		}
	}

	private void SeedUsers()
	{
		_users.AddRange(new[]
		{
			new User
			{
				Id = "admin",
				Email = "admin@lumenlibrary.com",
				Username = "admin",
				Password = "admin123",
				FullName = "Lumen Admin",
				IsAdmin = true
			},
			new User
			{
				Id = "maya",
				Email = "maya@readers.com",
				Username = "maya",
				Password = "reader123",
				FullName = "Maya Laurent",
				FavouriteCategories = { "fiction", "wellbeing" }
			},
			new User
			{
				Id = "leo",
				Email = "leo@readers.com",
				Username = "leo",
				Password = "reader123",
				FullName = "Leo Kim",
				FavouriteCategories = { "technology", "business" }
			}
		});
	}
}
