using System;
using System.Collections.Generic;
using System.Linq;
using LibraryWebApp.Models;

namespace LibraryWebApp.Services;

public class BookingService
{
	private readonly List<Booking> _bookings = new();
	private readonly object _mutex = new();
	private readonly BookService _bookService;

	public BookingService(BookService bookService)
	{
		_bookService = bookService;
	}

	public IEnumerable<Booking> GetAll() => _bookings.OrderByDescending(b => b.CreatedAt);

	public IEnumerable<Booking> GetRecent(int count = 5) => _bookings.OrderByDescending(b => b.CreatedAt).Take(count);

	public IEnumerable<Booking> GetForUser(string userId) =>
		_bookings.Where(b => b.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase))
				 .OrderByDescending(b => b.CreatedAt);

	public IEnumerable<Booking> GetHistory(string? status = null, string? search = null)
	{
		var query = _bookings.AsEnumerable();

		// Filter by status
		if (!string.IsNullOrWhiteSpace(status))
		{
			query = query.Where(b => b.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
		}

		// Search by book title or username
		if (!string.IsNullOrWhiteSpace(search))
		{
			query = query.Where(b => 
				b.BookTitle.Contains(search, StringComparison.OrdinalIgnoreCase) ||
				b.Username.Contains(search, StringComparison.OrdinalIgnoreCase));
		}

		return query.OrderByDescending(b => b.CreatedAt);
	}

	public IEnumerable<Booking> GetOverdueBookings()
	{
		return _bookings.Where(b => 
			b.Status == BookingStatus.Approved && 
			b.DueDate.HasValue && 
			b.DueDate.Value < DateTime.UtcNow)
			.OrderBy(b => b.DueDate);
	}

	public Booking? GetById(string id) => _bookings.FirstOrDefault(b => b.Id == id);

	public Booking ReserveBook(User user, Book book)
	{
		lock (_mutex)
		{
			if (HasActiveBookingForBook(book.Id))
			{
				throw new InvalidOperationException("This title already has an active reservation.");
			}

			var booking = new Booking
			{
				BookId = book.Id,
				BookTitle = book.Title,
				UserId = user.Id,
				Username = user.FullName,
				CreatedAt = DateTime.UtcNow,
				Status = BookingStatus.Pending
			};

			_bookings.Add(booking);
			_bookService.MarkReserved(book.Id);
			return booking;
		}
	}

	public bool ApproveBooking(string bookingId, DateTime dueDate)
	{
		lock (_mutex)
		{
			var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
			if (booking is null || booking.Status != BookingStatus.Pending)
			{
				return false;
			}

			booking.Status = BookingStatus.Approved;
			booking.ApprovedAt = DateTime.UtcNow;
			booking.DueDate = dueDate;
			_bookService.MarkReserved(booking.BookId);
			return true;
		}
	}

	public bool RefuseBooking(string bookingId)
	{
		lock (_mutex)
		{
			var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
			if (booking is null || booking.Status != BookingStatus.Pending)
			{
				return false;
			}

			booking.Status = BookingStatus.Refused;
			booking.ApprovedAt = null;
			booking.DueDate = null;
			booking.ReturnedAt = DateTime.UtcNow;
			_bookService.MarkReturned(booking.BookId);
			return true;
		}
	}

	public bool MarkReturned(string bookingId)
	{
		lock (_mutex)
		{
			var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
			if (booking is null || booking.Status != BookingStatus.Approved) return false;

			booking.Status = BookingStatus.Completed;
			booking.ReturnedAt = DateTime.UtcNow;
			_bookService.MarkReturned(booking.BookId);
			return true;
		}
	}

	private bool HasActiveBookingForBook(string bookId)
	{
		return _bookings.Any(b => b.BookId.Equals(bookId, StringComparison.OrdinalIgnoreCase)
			&& (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Approved));
	}
}
