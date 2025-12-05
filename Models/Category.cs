using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApp.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("categoryId")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [BsonElement("categoryName")]
        public string CategoryName { get; set; } = string.Empty;

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;
    }
}
