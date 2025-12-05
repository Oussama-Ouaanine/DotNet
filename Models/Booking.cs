using System;

namespace LibraryWebApp.Models;

public class Booking
{
	public string Id { get; set; } = Guid.NewGuid().ToString();
	public string BookId { get; set; } = string.Empty;
	public string BookTitle { get; set; } = string.Empty;
	public string UserId { get; set; } = string.Empty;
	public string Username { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public DateTime? ApprovedAt { get; set; }
	public DateTime? DueDate { get; set; }
	public DateTime? ReturnedAt { get; set; }
	public string Status { get; set; } = BookingStatus.Pending;
}
