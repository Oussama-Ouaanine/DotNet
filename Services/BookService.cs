using LibraryWebApp.Models;
using MongoDB.Driver;

namespace LibraryWebApp.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(MongoDbService mongoDbService)
        {
            _books = mongoDbService.Books;
        }

        public async Task<List<Book>> GetAllAsync() =>
            await _books.Find(_ => true).ToListAsync();

        public async Task<Book?> GetByIdAsync(string id) =>
            await _books.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Book>> GetByCategoryAsync(string category) =>
            await _books.Find(x => x.Category == category).ToListAsync();

        public async Task<List<Book>> SearchBooksAsync(string searchTerm)
        {
            var filter = Builders<Book>.Filter.Or(
                Builders<Book>.Filter.Regex(x => x.Title, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                Builders<Book>.Filter.Regex(x => x.Author, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                Builders<Book>.Filter.Regex(x => x.ISBN, new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
            );
            return await _books.Find(filter).ToListAsync();
        }

        public async Task<List<Book>> GetAvailableBooksAsync() =>
            await _books.Find(x => x.AvailableCopies > 0).ToListAsync();

        public async Task CreateAsync(Book book)
        {
            var maxBookId = await _books.Find(_ => true)
                .SortByDescending(b => b.BookId)
                .Limit(1)
                .FirstOrDefaultAsync();
            
            book.BookId = maxBookId?.BookId + 1 ?? 1;
            book.TotalCopies = book.AvailableCopies;
            await _books.InsertOneAsync(book);
        }

        public async Task UpdateAsync(string id, Book book) =>
            await _books.ReplaceOneAsync(x => x.Id == id, book);

        public async Task DeleteAsync(string id) =>
            await _books.DeleteOneAsync(x => x.Id == id);

        public async Task<bool> UpdateAvailabilityAsync(string id, int change)
        {
            var book = await GetByIdAsync(id);
            if (book == null) return false;

            book.AvailableCopies += change;
            
            if (book.AvailableCopies < 0)
            {
                book.AvailableCopies = 0;
                book.Status = "Unavailable";
            }
            else if (book.AvailableCopies > 0)
            {
                book.Status = "Available";
            }
            else
            {
                book.Status = "Unavailable";
            }

            await UpdateAsync(id, book);
            return true;
        }
    }
}
