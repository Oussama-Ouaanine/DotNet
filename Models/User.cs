using System;
using System.Collections.Generic;

namespace LibraryWebApp.Models;

public class User
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string Username { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string FullName { get; set; } = string.Empty;
	public bool IsAdmin { get; set; }
	public List<string> FavouriteCategories { get; set; } = new();
}
