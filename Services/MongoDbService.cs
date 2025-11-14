using LibraryWebApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryWebApp.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IOptions<DatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Book> Books => _database.GetCollection<Book>("Books");
        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Booking> Bookings => _database.GetCollection<Booking>("Bookings");
    }
}
