using LibraryWebApp.Models;
using MongoDB.Driver;

namespace LibraryWebApp.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(MongoDbService mongoDbService)
        {
            _users = mongoDbService.Users;
        }

        public async Task<List<User>> GetAllAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User?> GetByIdAsync(string id) =>
            await _users.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<User?> GetByUsernameAsync(string username) =>
            await _users.Find(x => x.Username == username).FirstOrDefaultAsync();

        public async Task<User?> GetByEmailAsync(string email) =>
            await _users.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task<List<User>> GetClientsAsync() =>
            await _users.Find(x => x.Role == "Client").ToListAsync();

        public async Task<List<User>> GetAdminsAsync() =>
            await _users.Find(x => x.Role == "Admin").ToListAsync();

        public async Task CreateAsync(User user)
        {
            var maxUserId = await _users.Find(_ => true)
                .SortByDescending(u => u.UserId)
                .Limit(1)
                .FirstOrDefaultAsync();
            
            user.UserId = maxUserId?.UserId + 1 ?? 1;
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateAsync(string id, User user) =>
            await _users.ReplaceOneAsync(x => x.Id == id, user);

        public async Task DeleteAsync(string id) =>
            await _users.DeleteOneAsync(x => x.Id == id);

        public async Task<User?> AuthenticateAsync(string username, string password) =>
            await _users.Find(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();
    }
}
