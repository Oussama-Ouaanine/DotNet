using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApp.Models
{
    public class Booking
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("bookingId")]
        public int BookingId { get; set; }

        [Required]
        [BsonElement("userId")]
        public int UserId { get; set; }

        [BsonElement("userObjectId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserObjectId { get; set; }

        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [BsonElement("bookId")]
        public int BookId { get; set; }

        [BsonElement("bookObjectId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? BookObjectId { get; set; }

        [BsonElement("bookTitle")]
        public string BookTitle { get; set; } = string.Empty;

        [BsonElement("bookingDate")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [BsonElement("dueDate")]
        public DateTime DueDate { get; set; }

        [BsonElement("returnDate")]
        public DateTime? ReturnDate { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Declined, Active, Returned, Overdue

        [BsonElement("lateFee")]
        public decimal LateFee { get; set; } = 0;
    }
}
