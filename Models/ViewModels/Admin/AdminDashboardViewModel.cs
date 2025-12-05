using System.Collections.Generic;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Admin;

public class AdminDashboardViewModel
{
    public int TotalBooks { get; set; }
    public int TotalCategories { get; set; }
    public int ActiveBookings { get; set; }
    public int TotalMembers { get; set; }
    public IEnumerable<Booking> RecentBookings { get; set; } = new List<Booking>();
    public IEnumerable<Book> FeaturedBooks { get; set; } = new List<Book>();
}
