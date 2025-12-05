using LibraryWebApp.Models;
using MongoDB.Driver;

namespace LibraryWebApp.Services
{
    public class BookingService
    {
        private readonly IMongoCollection<Booking> _bookings;
        private readonly BookService _bookService;

        public BookingService(MongoDbService mongoDbService, BookService bookService)
        {
            _bookings = mongoDbService.Bookings;
            _bookService = bookService;
        }

        public async Task<List<Booking>> GetAllAsync() =>
            await _bookings.Find(_ => true).SortByDescending(b => b.BookingDate).ToListAsync();

        public async Task<Booking?> GetByIdAsync(string id) =>
            await _bookings.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Booking>> GetByUserIdAsync(int userId) =>
            await _bookings.Find(x => x.UserId == userId).SortByDescending(b => b.BookingDate).ToListAsync();

        public async Task<List<Booking>> GetPendingBookingsAsync() =>
            await _bookings.Find(x => x.Status == "Pending").SortByDescending(b => b.BookingDate).ToListAsync();

        public async Task<List<Booking>> GetActiveBookingsAsync() =>
            await _bookings.Find(x => x.Status == "Active").ToListAsync();

        public async Task CreateAsync(Booking booking)
        {
            var maxBookingId = await _bookings.Find(_ => true)
                .SortByDescending(b => b.BookingId)
                .Limit(1)
                .FirstOrDefaultAsync();
            
            booking.BookingId = maxBookingId?.BookingId + 1 ?? 1;
            booking.DueDate = booking.BookingDate.AddDays(14); // Default 14 days loan period
            await _bookings.InsertOneAsync(booking);
        }

        public async Task UpdateAsync(string id, Booking booking) =>
            await _bookings.ReplaceOneAsync(x => x.Id == id, booking);

        public async Task DeleteAsync(string id) =>
            await _bookings.DeleteOneAsync(x => x.Id == id);

        public async Task<bool> ApproveBookingAsync(string id)
        {
            var booking = await GetByIdAsync(id);
            if (booking == null || booking.Status != "Pending") return false;

            booking.Status = "Active";
            await UpdateAsync(id, booking);
            
            // Decrease available copies
            if (!string.IsNullOrEmpty(booking.BookObjectId))
            {
                await _bookService.UpdateAvailabilityAsync(booking.BookObjectId, -1);
            }
            
            return true;
        }

        public async Task<bool> DeclineBookingAsync(string id)
        {
            var booking = await GetByIdAsync(id);
            if (booking == null || booking.Status != "Pending") return false;

            booking.Status = "Declined";
            await UpdateAsync(id, booking);
            return true;
        }

        public async Task<bool> ReturnBookAsync(string id)
        {
            var booking = await GetByIdAsync(id);
            if (booking == null || booking.Status != "Active") return false;

            booking.ReturnDate = DateTime.Now;
            booking.Status = "Returned";
            
            // Calculate late fee if overdue
            if (booking.ReturnDate > booking.DueDate)
            {
                var daysLate = (booking.ReturnDate.Value - booking.DueDate).Days;
                booking.LateFee = daysLate * 1.0m; // $1 per day late fee
            }

            await UpdateAsync(id, booking);
            
            // Increase available copies
            if (!string.IsNullOrEmpty(booking.BookObjectId))
            {
                await _bookService.UpdateAvailabilityAsync(booking.BookObjectId, 1);
            }
            
            return true;
        }

        public async Task CheckAndUpdateOverdueBookingsAsync()
        {
            var activeBookings = await GetActiveBookingsAsync();
            foreach (var booking in activeBookings)
            {
                if (booking.DueDate < DateTime.Now && booking.Status == "Active")
                {
                    booking.Status = "Overdue";
                    await UpdateAsync(booking.Id!, booking);
                }
            }
        }
    }
}
