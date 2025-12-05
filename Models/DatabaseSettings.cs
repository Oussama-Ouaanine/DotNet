namespace LibraryWebApp.Models
{
    public class DatabaseSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string DatabaseName { get; set; } = string.Empty;
        public string UsersCollectionName { get; set; } = "Users";
        public string BooksCollectionName { get; set; } = "Books";
        public string BookingsCollectionName { get; set; } = "Bookings";
        public string CategoriesCollectionName { get; set; } = "Categories";
    }
}
