using System.Collections.Generic;
using LibraryWebApp.Models;

namespace LibraryWebApp.Models.ViewModels.Client;

public class MyBookingsViewModel
{
    public IEnumerable<Booking> Pending { get; set; } = new List<Booking>();
    public IEnumerable<Booking> Approved { get; set; } = new List<Booking>();
    public IEnumerable<Booking> History { get; set; } = new List<Booking>();
}
