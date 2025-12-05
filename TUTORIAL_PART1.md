# 🎓 Complete Step-by-Step Tutorial: Building a Library Management System with ASP.NET Core MVC and MongoDB

## 📚 Table of Contents

1. [Introduction](#introduction)
2. [Prerequisites](#prerequisites)
3. [Understanding the Architecture](#understanding-the-architecture)
4. [Step 1: Project Setup](#step-1-project-setup)
5. [Step 2: Understanding Models](#step-2-understanding-models)
6. [Step 3: MongoDB Integration](#step-3-mongodb-integration)
7. [Step 4: Creating Services](#step-4-creating-services)
8. [Step 5: Building Controllers](#step-5-building-controllers)
9. [Step 6: Creating Views](#step-6-creating-views)
10. [Step 7: Authentication & Authorization](#step-7-authentication--authorization)
11. [Step 8: Testing](#step-8-testing)
12. [Key Concepts Explained](#key-concepts-explained)
13. [Common Patterns](#common-patterns)
14. [Troubleshooting](#troubleshooting)
15. [Next Steps](#next-steps)

---

## Introduction

This tutorial will teach you how to build a complete web application using:
- **ASP.NET Core MVC** - A framework for building web applications
- **MongoDB** - A NoSQL database
- **C#** - The programming language
- **Bootstrap** - For responsive UI

By the end, you'll understand:
- MVC architecture pattern
- Database integration
- Authentication & authorization
- CRUD operations (Create, Read, Update, Delete)
- Routing and navigation

---

## Prerequisites

### What You Need Installed
```bash
# Check .NET SDK
dotnet --version  # Should be 8.0 or higher

# Check MongoDB
mongod --version  # Should be installed and running
```

### Knowledge Prerequisites
- Basic C# syntax (variables, methods, classes)
- Basic HTML/CSS
- Understanding of databases (tables/collections, CRUD)
- Basic command line usage

Don't worry if you're not an expert - I'll explain everything!

---

## Understanding the Architecture

### MVC Pattern Explained

**MVC = Model-View-Controller**

```
┌─────────────────────────────────────────┐
│           User (Browser)                │
└────────────┬────────────────────────────┘
             │
             ↓
┌────────────────────────────────────────┐
│         CONTROLLER                      │
│  - Receives user requests               │
│  - Processes logic                      │
│  - Calls services/models                │
│  - Returns views                        │
└────────────┬───────────────────────────┘
             │
    ┌────────┴────────┐
    ↓                 ↓
┌─────────┐    ┌──────────┐
│  MODEL  │    │   VIEW   │
│  - Data │    │  - HTML  │
│  - Logic│    │  - UI    │
└─────────┘    └──────────┘
```

**Example Flow:**
1. User clicks "View Books"
2. **Controller** receives request
3. **Controller** asks **Model** for book data
4. **Model** gets data from database
5. **Controller** passes data to **View**
6. **View** renders HTML
7. User sees the books page

### Our Project Structure

```
LibraryWebApp/
├── Models/              # Data structures (User, Book, etc.)
├── Services/            # Database operations (CRUD)
├── Controllers/         # Handle requests, business logic
├── Views/              # HTML pages (Razor)
├── wwwroot/            # Static files (CSS, JS, images)
├── Program.cs          # Application startup
└── appsettings.json    # Configuration
```

---

## Step 1: Project Setup

### 1.1 Create a New ASP.NET Core MVC Project

```bash
# Navigate to your workspace
cd /home/ergo/Desktop/dot-net

# Create new MVC project
dotnet new mvc -n LibraryWebApp

# Navigate into project
cd LibraryWebApp
```

**What this does:**
- `dotnet new mvc` - Creates a new MVC project template
- `-n LibraryWebApp` - Names your project "LibraryWebApp"
- Creates all necessary folders and files

### 1.2 Understand the Generated Files

```
LibraryWebApp/
├── Controllers/
│   └── HomeController.cs        # Default controller
├── Models/
│   └── ErrorViewModel.cs        # Error handling model
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml         # Home page
│   │   └── Privacy.cshtml       # Privacy page
│   └── Shared/
│       ├── _Layout.cshtml       # Master layout
│       └── Error.cshtml         # Error page
├── wwwroot/                     # Static files
├── Program.cs                   # Entry point
└── LibraryWebApp.csproj        # Project file
```

### 1.3 Add MongoDB Package

```bash
# Add MongoDB driver
dotnet add package MongoDB.Driver
```

**What this does:**
- Downloads MongoDB.Driver library
- Allows your app to connect to MongoDB
- Adds reference to your `.csproj` file

### 1.4 Run the Default Project

```bash
# Build the project
dotnet build

# Run the project
dotnet run
```

Open browser: http://localhost:5284

You should see the default ASP.NET welcome page.

---

## Step 2: Understanding Models

### 2.1 What are Models?

**Models are C# classes that represent your data.**

Think of a model like a blueprint:
- A **User** model defines what a user looks like (username, email, password)
- A **Book** model defines what a book looks like (title, author, ISBN)

### 2.2 Creating the User Model

Create `Models/User.cs`:

```csharp
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace LibraryWebApp.Models
{
    public class User
    {
        // MongoDB ID (unique identifier)
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        // Integer ID for easy reference
        [BsonElement("userId")]
        public int UserId { get; set; }

        // Username field (required)
        [Required(ErrorMessage = "Username is required")]
        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        // Password field (required)
        [Required(ErrorMessage = "Password is required")]
        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        // Email field (required, must be valid email)
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        // Role: "Admin" or "Client"
        [Required]
        [BsonElement("role")]
        public string Role { get; set; } = "Client";

        // When user registered
        [BsonElement("registrationDate")]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        // Admin-specific field
        [BsonElement("adminLevel")]
        public string? AdminLevel { get; set; }

        // Client-specific fields
        [BsonElement("phoneNumber")]
        public string? PhoneNumber { get; set; }

        [BsonElement("address")]
        public string? Address { get; set; }
    }
}
```

**Understanding the Attributes:**

- `[BsonId]` - Tells MongoDB this is the unique identifier
- `[BsonElement("fieldName")]` - Maps C# property to MongoDB field
- `[Required]` - Makes field mandatory
- `[EmailAddress]` - Validates email format
- `string?` - Means the field is optional (nullable)
- `string.Empty` - Default empty string value

### 2.3 Creating the Book Model

Create `Models/Book.cs`:

```csharp
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

        [Required(ErrorMessage = "ISBN is required")]
        [BsonElement("ISBN")]
        public string ISBN { get; set; } = string.Empty;

        [Required]
        [BsonElement("publicationYear")]
        public int PublicationYear { get; set; }

        [BsonElement("description")]
        public string Description { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = "Available";

        [Required]
        [BsonElement("availableCopies")]
        public int AvailableCopies { get; set; }

        [BsonElement("totalCopies")]
        public int TotalCopies { get; set; }
    }
}
```

### 2.4 Exercise: Create Category Model Yourself

**Try to create `Models/Category.cs` yourself before looking at the solution:**

Requirements:
- CategoryId (int)
- CategoryName (string, required)
- Description (string)

<details>
<summary>Click to see solution</summary>

```csharp
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
```

</details>

---

## Step 3: MongoDB Integration

### 3.1 Understanding MongoDB Connection

MongoDB is a **NoSQL database** that stores data in **collections** (like tables) containing **documents** (like rows).

Our app needs:
1. **Connection String** - Where is MongoDB? (mongodb://localhost:27017)
2. **Database Name** - Which database to use? (LibraryDB)
3. **Collection Names** - Which collections to use? (Users, Books, etc.)

### 3.2 Create DatabaseSettings Model

Create `Models/DatabaseSettings.cs`:

```csharp
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
```

### 3.3 Configure Database Settings

Edit `appsettings.json`:

```json
{
  "DatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "LibraryDB",
    "UsersCollectionName": "Users",
    "BooksCollectionName": "Books",
    "BookingsCollectionName": "Bookings",
    "CategoriesCollectionName": "Categories"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**What this does:**
- Stores configuration outside of code
- Can be changed without recompiling
- Can have different settings for development/production

### 3.4 Create MongoDB Service

Create `Services/MongoDbService.cs`:

```csharp
using LibraryWebApp.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace LibraryWebApp.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        // Constructor: runs when service is created
        public MongoDbService(IOptions<DatabaseSettings> settings)
        {
            // Create MongoDB client
            var client = new MongoClient(settings.Value.ConnectionString);
            
            // Get database
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        // Properties to access collections
        public IMongoCollection<User> Users => 
            _database.GetCollection<User>("Users");
            
        public IMongoCollection<Book> Books => 
            _database.GetCollection<Book>("Books");
            
        public IMongoCollection<Category> Categories => 
            _database.GetCollection<Category>("Categories");
            
        public IMongoCollection<Booking> Bookings => 
            _database.GetCollection<Booking>("Bookings");
    }
}
```

**Understanding the Code:**

- `IOptions<DatabaseSettings>` - Injects settings from appsettings.json
- `MongoClient` - Connects to MongoDB
- `IMongoDatabase` - Represents the database
- `IMongoCollection<T>` - Represents a collection (like a table)
- `=>` - Property getter (shorthand for get method)

### 3.5 Register Services in Program.cs

Edit `Program.cs`:

```csharp
using LibraryWebApp.Models;
using LibraryWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure MongoDB settings from appsettings.json
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("DatabaseSettings"));

// Register MongoDB service as singleton (one instance for entire app)
builder.Services.AddSingleton<MongoDbService>();

// Add other services (we'll add more later)
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<BookService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<BookingService>();

// Add session support for authentication
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

**Understanding Dependency Injection:**

```csharp
builder.Services.AddSingleton<MongoDbService>();
```

This tells ASP.NET Core:
- "When someone needs `MongoDbService`, create one instance"
- "Keep that instance for the entire application lifetime"
- "Inject it automatically into constructors that need it"

---

## Step 4: Creating Services

Services contain the **business logic** and **database operations**.

### 4.1 Understanding the Service Pattern

**Why Services?**
- Separates database logic from controllers
- Makes code reusable
- Easier to test
- Follows Single Responsibility Principle

```
Controller → Service → Database
```

### 4.2 Create UserService

Create `Services/UserService.cs`:

```csharp
using LibraryWebApp.Models;
using MongoDB.Driver;

namespace LibraryWebApp.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        // Constructor: inject MongoDbService
        public UserService(MongoDbService mongoDbService)
        {
            _users = mongoDbService.Users;
        }

        // GET ALL USERS
        public async Task<List<User>> GetAllAsync()
        {
            // Find all users, return as list
            return await _users.Find(_ => true).ToListAsync();
        }

        // GET USER BY ID
        public async Task<User?> GetByIdAsync(string id)
        {
            // Find user where Id equals provided id
            return await _users.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        // GET USER BY USERNAME
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _users.Find(x => x.Username == username).FirstOrDefaultAsync();
        }

        // GET CLIENTS ONLY
        public async Task<List<User>> GetClientsAsync()
        {
            return await _users.Find(x => x.Role == "Client").ToListAsync();
        }

        // CREATE NEW USER
        public async Task CreateAsync(User user)
        {
            // Get the highest userId
            var maxUserId = await _users.Find(_ => true)
                .SortByDescending(u => u.UserId)
                .Limit(1)
                .FirstOrDefaultAsync();
            
            // Set new user's ID (increment by 1)
            user.UserId = maxUserId?.UserId + 1 ?? 1;
            
            // Insert into database
            await _users.InsertOneAsync(user);
        }

        // UPDATE USER
        public async Task UpdateAsync(string id, User user)
        {
            // Replace entire user document
            await _users.ReplaceOneAsync(x => x.Id == id, user);
        }

        // DELETE USER
        public async Task DeleteAsync(string id)
        {
            await _users.DeleteOneAsync(x => x.Id == id);
        }

        // AUTHENTICATE USER
        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _users.Find(x => 
                x.Username == username && x.Password == password
            ).FirstOrDefaultAsync();
        }
    }
}
```

**Understanding Async/Await:**

```csharp
public async Task<List<User>> GetAllAsync()
{
    return await _users.Find(_ => true).ToListAsync();
}
```

- `async` - This method is asynchronous
- `Task<T>` - Returns a task that will eventually have result of type T
- `await` - Wait for database operation to complete
- **Why?** - Don't block the thread while waiting for database

**Understanding Lambda Expressions:**

```csharp
x => x.Username == username
```

This is a shorthand function:
```csharp
bool CheckUsername(User x)
{
    return x.Username == username;
}
```

### 4.3 Exercise: Create BookService Yourself

**Try to create `Services/BookService.cs` with these methods:**
- `GetAllAsync()` - Get all books
- `GetByIdAsync(string id)` - Get book by ID
- `CreateAsync(Book book)` - Create new book
- `SearchBooksAsync(string searchTerm)` - Search by title/author

<details>
<summary>Click to see solution</summary>

```csharp
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

        public async Task<List<Book>> SearchBooksAsync(string searchTerm)
        {
            var filter = Builders<Book>.Filter.Or(
                Builders<Book>.Filter.Regex(x => x.Title, 
                    new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                Builders<Book>.Filter.Regex(x => x.Author, 
                    new MongoDB.Bson.BsonRegularExpression(searchTerm, "i"))
            );
            return await _books.Find(filter).ToListAsync();
        }

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
    }
}
```

</details>

---

## Step 5: Building Controllers

Controllers handle HTTP requests and return responses.

### 5.1 Understanding Controllers

**Request Flow:**
```
Browser → Route → Controller → Service → Database
                       ↓
                     View
                       ↓
Browser ← HTML ← Controller
```

### 5.2 Understanding Routing

**URL Pattern:** `/{controller}/{action}/{id?}`

Examples:
- `/Home/Index` → HomeController.Index()
- `/Account/Login` → AccountController.Login()
- `/Admin/Books` → AdminController.Books()
- `/Client/BookDetails/5` → ClientController.BookDetails(5)

### 5.3 Create AccountController

Create `Controllers/AccountController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Models;
using LibraryWebApp.Services;

namespace LibraryWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        // Constructor: inject UserService
        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Return Login view
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Username and password are required";
                return View();
            }

            // Authenticate user
            var user = await _userService.AuthenticateAsync(username, password);
            
            if (user == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            // Store user info in session
            HttpContext.Session.SetString("UserId", user.Id!);
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetInt32("UserIdInt", user.UserId);

            // Redirect based on role
            if (user.Role == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return RedirectToAction("Index", "Client");
            }
        }

        // GET: /Account/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
```

**Understanding Action Methods:**

```csharp
[HttpGet]
public IActionResult Login()
{
    return View();
}
```

- `[HttpGet]` - Responds to GET requests (browser navigation)
- `IActionResult` - Can return different types (View, Redirect, JSON)
- `View()` - Returns `Views/Account/Login.cshtml`

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Login(string username, string password)
{
    // Process form submission
}
```

- `[HttpPost]` - Responds to POST requests (form submissions)
- `[ValidateAntiForgeryToken]` - Security: prevents CSRF attacks
- Parameters automatically bound from form data

**Understanding ViewBag:**

```csharp
ViewBag.Error = "Invalid username";
```

- Passes data from controller to view
- Dynamic property bag
- Access in view: `@ViewBag.Error`

**Understanding RedirectToAction:**

```csharp
return RedirectToAction("Index", "Admin");
```

- Redirects to another action
- First parameter: action name
- Second parameter: controller name
- Result: `/Admin/Index`

---

*This tutorial continues in PART 2...*

Would you like me to continue with the remaining sections?
