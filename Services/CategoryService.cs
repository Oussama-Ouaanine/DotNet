using LibraryWebApp.Models;
using MongoDB.Driver;

namespace LibraryWebApp.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categories;

        public CategoryService(MongoDbService mongoDbService)
        {
            _categories = mongoDbService.Categories;
        }

        public async Task<List<Category>> GetAllAsync() =>
            await _categories.Find(_ => true).ToListAsync();

        public async Task<Category?> GetByIdAsync(string id) =>
            await _categories.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<Category?> GetByNameAsync(string name) =>
            await _categories.Find(x => x.CategoryName == name).FirstOrDefaultAsync();

        public async Task CreateAsync(Category category)
        {
            var maxCategoryId = await _categories.Find(_ => true)
                .SortByDescending(c => c.CategoryId)
                .Limit(1)
                .FirstOrDefaultAsync();
            
            category.CategoryId = maxCategoryId?.CategoryId + 1 ?? 1;
            await _categories.InsertOneAsync(category);
        }

        public async Task UpdateAsync(string id, Category category) =>
            await _categories.ReplaceOneAsync(x => x.Id == id, category);

        public async Task DeleteAsync(string id) =>
            await _categories.DeleteOneAsync(x => x.Id == id);
    }
}
