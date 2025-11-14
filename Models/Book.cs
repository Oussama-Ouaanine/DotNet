using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApp.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("bookId")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        [BsonElement("author")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        [BsonElement("category")]
        public string Category { get; set; } = string.Empty;

        [BsonElement("categoryId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CategoryId { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        [BsonElement("ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [BsonElement("publicationYear")]
        public int PublicationYear { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "Available"; // Available, Borrowed, Reserved

        [Required]
        [BsonElement("availableCopies")]
        public int AvailableCopies { get; set; }

        [BsonElement("totalCopies")]
        public int TotalCopies { get; set; }

        [BsonElement("imagePath")]
        public string? ImagePath { get; set; } // Path to book cover image
    }
}
