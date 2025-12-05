using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApp.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("userId")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [BsonElement("role")]
        public string Role { get; set; } = "Client"; // "Admin" or "Client"

        [BsonElement("registrationDate")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Admin specific fields
        [BsonElement("adminLevel")]
        public string? AdminLevel { get; set; }

        // Client specific fields
        [BsonElement("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }
    }
}
