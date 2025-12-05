# 🎓 Complete Step-by-Step Tutorial: Building a Library Management System - PART 2

## Continuation from Part 1...

## Step 5 (Continued): Building Controllers

### 5.4 Create AdminController

Create `Controllers/AdminController.cs`:

```csharp
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Models;
using LibraryWebApp.Services;

namespace LibraryWebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly BookService _bookService;
        private readonly CategoryService _categoryService;
        private readonly BookingService _bookingService;
        private readonly UserService _userService;

        // Constructor: inject multiple services
        public AdminController(
            BookService bookService, 
            CategoryService categoryService,
            BookingService bookingService, 
            UserService userService)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _bookingService = bookingService;
            _userService = userService;
        }

        // Helper method to check if user is admin
        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Admin";
        }

        // GET: /Admin/Index (Dashboard)
        public async Task<IActionResult> Index()
        {
            // Check authorization
            if (!IsAdmin()) 
                return RedirectToAction("Login", "Account");

            // Get statistics
            ViewBag.TotalBooks = (await _bookService.GetAllAsync()).Count;
            ViewBag.TotalUsers = (await _userService.GetClientsAsync()).Count;
            ViewBag.PendingBookings = (await _bookingService.GetPendingBookingsAsync()).Count;
            ViewBag.ActiveBookings = (await _bookingService.GetActiveBookingsAsync()).Count;

            return View();
        }

        // GET: /Admin/Books
        public async Task<IActionResult> Books()
        {
            if (!IsAdmin()) 
                return RedirectToAction("Login", "Account");

            var books = await _bookService.GetAllAsync();
            return View(books);  // Pass books to view
        }

        // GET: /Admin/CreateBook
        public async Task<IActionResult> CreateBook()
        {
            if (!IsAdmin()) 
                return RedirectToAction("Login", "Account");

            // Pass categories to view for dropdown
            ViewBag.Categories = await _categoryService.GetAllAsync();
            return View();
        }

        // POST: /Admin/CreateBook
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBook(Book book)
        {
            if (!IsAdmin()) 
                return RedirectToAction("Login", "Account");

            // Check if model is valid
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _categoryService.GetAllAsync();
                return View(book);
            }

            await _bookService.CreateAsync(book);
            return RedirectToAction("Books");
        }

        // POST: /Admin/DeleteBook/5
        [HttpPost]
        public async Task<IActionResult> DeleteBook(string id)
        {
            if (!IsAdmin()) 
                return RedirectToAction("Login", "Account");

            await _bookService.DeleteAsync(id);
            return RedirectToAction("Books");
        }
    }
}
```

**Understanding ModelState:**

```csharp
if (!ModelState.IsValid)
{
    return View(book);
}
```

- Validates model based on attributes like `[Required]`
- If invalid, returns view with error messages
- Errors displayed using validation helpers in view

**Understanding Multiple Dependencies:**

```csharp
public AdminController(
    BookService bookService, 
    CategoryService categoryService)
{
    _bookService = bookService;
    _categoryService = categoryService;
}
```

- ASP.NET Core automatically injects all services
- You can inject as many as you need

### 5.5 Exercise: Create ClientController

**Try creating a simple ClientController with:**
- `Index()` - Show available books
- `MyBookings()` - Show user's bookings

<details>
<summary>Click to see solution</summary>

```csharp
using Microsoft.AspNetCore.Mvc;
using LibraryWebApp.Services;

namespace LibraryWebApp.Controllers
{
    public class ClientController : Controller
    {
        private readonly BookService _bookService;
        private readonly BookingService _bookingService;

        public ClientController(BookService bookService, BookingService bookingService)
        {
            _bookService = bookService;
            _bookingService = bookingService;
        }

        private bool IsClient()
        {
            var role = HttpContext.Session.GetString("Role");
            return role == "Client";
        }

        private int GetUserId()
        {
            return HttpContext.Session.GetInt32("UserIdInt") ?? 0;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsClient()) 
                return RedirectToAction("Login", "Account");

            var books = await _bookService.GetAvailableBooksAsync();
            return View(books);
        }

        public async Task<IActionResult> MyBookings()
        {
            if (!IsClient()) 
                return RedirectToAction("Login", "Account");

            var userId = GetUserId();
            var bookings = await _bookingService.GetByUserIdAsync(userId);
            return View(bookings);
        }
    }
}
```

</details>

---

## Step 6: Creating Views

Views are the HTML pages users see.

### 6.1 Understanding Razor Syntax

**Razor** mixes HTML with C# code using `@` symbol.

```razor
<h1>Hello, World!</h1>            <!-- Regular HTML -->

<h1>Hello, @name!</h1>             <!-- Razor: C# variable -->

@if (isAdmin)                      <!-- Razor: C# if statement -->
{
    <p>You are an admin!</p>
}

@foreach (var book in books)       <!-- Razor: C# loop -->
{
    <p>@book.Title</p>
}
```

### 6.2 Understanding the Layout System

**Master Layout:**
`Views/Shared/_Layout.cshtml` - Contains header, footer, navigation

**Individual Views:**
Content is inserted into `@RenderBody()`

```
┌─────────────────────────────────┐
│   Header (from _Layout)         │
├─────────────────────────────────┤
│                                 │
│   @RenderBody()                 │
│   (Your page content here)      │
│                                 │
├─────────────────────────────────┤
│   Footer (from _Layout)         │
└─────────────────────────────────┘
```

### 6.3 Edit the Layout

Edit `Views/Shared/_Layout.cshtml` to add navigation:

```razor
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Library</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" />
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-light bg-white border-bottom">
            <div class="container-fluid">
                <a class="navbar-brand" asp-controller="Home" asp-action="Index">
                    📚 Library System
                </a>
                <button class="navbar-toggler" type="button" data-bs-toggle="collapse" 
                        data-bs-target=".navbar-collapse">
                    <span class="navbar-toggler-icon"></span>
                </button>
                <div class="navbar-collapse collapse">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link" asp-controller="Home" asp-action="Index">
                                Home
                            </a>
                        </li>
                        
                        @* Show admin menu if user is admin *@
                        @if (Context.Session.GetString("Role") == "Admin")
                        {
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Admin" asp-action="Index">
                                    Dashboard
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Admin" asp-action="Books">
                                    Books
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Admin" asp-action="Bookings">
                                    Bookings
                                </a>
                            </li>
                        }
                        
                        @* Show client menu if user is client *@
                        @if (Context.Session.GetString("Role") == "Client")
                        {
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Client" asp-action="Browse">
                                    Browse Books
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Client" asp-action="MyBookings">
                                    My Bookings
                                </a>
                            </li>
                        }
                    </ul>
                    
                    @* User info / Login links *@
                    <ul class="navbar-nav">
                        @if (Context.Session.GetString("Username") != null)
                        {
                            <li class="nav-item">
                                <span class="nav-link">
                                    👤 @Context.Session.GetString("Username")
                                </span>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Account" asp-action="Logout">
                                    Logout
                                </a>
                            </li>
                        }
                        else
                        {
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Account" asp-action="Login">
                                    Login
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" asp-controller="Account" asp-action="Register">
                                    Register
                                </a>
                            </li>
                        }
                    </ul>
                </div>
            </div>
        </nav>
    </header>
    
    <div class="container mt-4">
        <main role="main">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted mt-5">
        <div class="container text-center py-3">
            &copy; 2025 - Library Management System
        </div>
    </footer>
    
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

**Understanding Tag Helpers:**

```razor
<a asp-controller="Home" asp-action="Index">Home</a>
```

- `asp-controller` - Target controller
- `asp-action` - Target action
- Generates: `<a href="/Home/Index">Home</a>`
- Benefits: Type-safe, auto-updates URLs

### 6.4 Create Login View

Create `Views/Account/Login.cshtml`:

```razor
@{
    ViewData["Title"] = "Login";
}

<div class="row justify-content-center">
    <div class="col-md-6">
        <div class="card">
            <div class="card-header bg-primary text-white">
                <h2>Login</h2>
            </div>
            <div class="card-body">
                @* Show error message if exists *@
                @if (ViewBag.Error != null)
                {
                    <div class="alert alert-danger">
                        @ViewBag.Error
                    </div>
                }
                
                <form method="post" asp-action="Login">
                    @* Anti-forgery token for security *@
                    @Html.AntiForgeryToken()
                    
                    <div class="mb-3">
                        <label for="username" class="form-label">Username</label>
                        <input type="text" class="form-control" id="username" 
                               name="username" required />
                    </div>
                    
                    <div class="mb-3">
                        <label for="password" class="form-label">Password</label>
                        <input type="password" class="form-control" id="password" 
                               name="password" required />
                    </div>
                    
                    <button type="submit" class="btn btn-primary">Login</button>
                    <a asp-action="Register" class="btn btn-link">
                        Don't have an account? Register
                    </a>
                </form>
            </div>
        </div>
    </div>
</div>
```

**Understanding Forms:**

```razor
<form method="post" asp-action="Login">
    @Html.AntiForgeryToken()
    <input name="username" />
    <input name="password" />
    <button type="submit">Login</button>
</form>
```

When submitted:
1. POST request to `/Account/Login`
2. Parameters match form field names
3. Anti-forgery token validated
4. Controller receives `username` and `password`

### 6.5 Create Books List View

Create `Views/Admin/Books.cshtml`:

```razor
@model IEnumerable<LibraryWebApp.Models.Book>

@{
    ViewData["Title"] = "Manage Books";
}

<div class="d-flex justify-content-between align-items-center mb-4">
    <h1>Manage Books</h1>
    <a asp-action="CreateBook" class="btn btn-primary">
        ➕ Add New Book
    </a>
</div>

@if (!Model.Any())
{
    <div class="alert alert-info">
        No books found. Click "Add New Book" to create one.
    </div>
}
else
{
    <table class="table table-striped table-hover">
        <thead class="table-dark">
            <tr>
                <th>Title</th>
                <th>Author</th>
                <th>Category</th>
                <th>ISBN</th>
                <th>Available</th>
                <th>Status</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var book in Model)
            {
                <tr>
                    <td>@book.Title</td>
                    <td>@book.Author</td>
                    <td>@book.Category</td>
                    <td>@book.ISBN</td>
                    <td>@book.AvailableCopies / @book.TotalCopies</td>
                    <td>
                        @if (book.AvailableCopies > 0)
                        {
                            <span class="badge bg-success">Available</span>
                        }
                        else
                        {
                            <span class="badge bg-danger">Unavailable</span>
                        }
                    </td>
                    <td>
                        <a asp-action="EditBook" asp-route-id="@book.Id" 
                           class="btn btn-sm btn-warning">
                            ✏️ Edit
                        </a>
                        <form method="post" asp-action="DeleteBook" 
                              asp-route-id="@book.Id" style="display:inline;">
                            <button type="submit" class="btn btn-sm btn-danger" 
                                    onclick="return confirm('Delete this book?')">
                                🗑️ Delete
                            </button>
                        </form>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}
```

**Understanding @model:**

```razor
@model IEnumerable<LibraryWebApp.Models.Book>
```

- Declares the type of data passed from controller
- Provides IntelliSense in Visual Studio/VS Code
- Access with `Model` (capital M)

**Understanding foreach:**

```razor
@foreach (var book in Model)
{
    <tr>
        <td>@book.Title</td>
    </tr>
}
```

- Loops through each book
- Generates one table row per book

### 6.6 Create Book Form View

Create `Views/Admin/CreateBook.cshtml`:

```razor
@model LibraryWebApp.Models.Book

@{
    ViewData["Title"] = "Add New Book";
}

<h1>Add New Book</h1>

<div class="row">
    <div class="col-md-8">
        <form method="post" asp-action="CreateBook">
            @Html.AntiForgeryToken()
            
            <div class="mb-3">
                <label asp-for="Title" class="form-label"></label>
                <input asp-for="Title" class="form-control" />
                <span asp-validation-for="Title" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <label asp-for="Author" class="form-label"></label>
                <input asp-for="Author" class="form-control" />
                <span asp-validation-for="Author" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <label asp-for="Category" class="form-label"></label>
                <select asp-for="Category" class="form-select">
                    <option value="">-- Select Category --</option>
                    @foreach (var category in ViewBag.Categories as List<LibraryWebApp.Models.Category>)
                    {
                        <option value="@category.CategoryName">
                            @category.CategoryName
                        </option>
                    }
                </select>
                <span asp-validation-for="Category" class="text-danger"></span>
            </div>
            
            <div class="mb-3">
                <label asp-for="ISBN" class="form-label"></label>
                <input asp-for="ISBN" class="form-control" />
                <span asp-validation-for="ISBN" class="text-danger"></span>
            </div>
            
            <div class="row">
                <div class="col-md-6">
                    <div class="mb-3">
                        <label asp-for="PublicationYear" class="form-label"></label>
                        <input asp-for="PublicationYear" class="form-control" type="number" />
                        <span asp-validation-for="PublicationYear" class="text-danger"></span>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="mb-3">
                        <label asp-for="AvailableCopies" class="form-label"></label>
                        <input asp-for="AvailableCopies" class="form-control" type="number" />
                        <span asp-validation-for="AvailableCopies" class="text-danger"></span>
                    </div>
                </div>
            </div>
            
            <div class="mb-3">
                <label asp-for="Description" class="form-label"></label>
                <textarea asp-for="Description" class="form-control" rows="4"></textarea>
            </div>
            
            <div class="d-flex gap-2">
                <button type="submit" class="btn btn-primary">💾 Create Book</button>
                <a asp-action="Books" class="btn btn-secondary">❌ Cancel</a>
            </div>
        </form>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

**Understanding Model Binding:**

```razor
<input asp-for="Title" class="form-control" />
```

Generates:
```html
<input type="text" id="Title" name="Title" class="form-control" />
```

- `asp-for` binds to model property
- Automatically sets name, id, and value
- Works with validation

**Understanding Validation:**

```razor
<span asp-validation-for="Title" class="text-danger"></span>
```

- Shows validation error messages
- Works with `[Required]`, `[EmailAddress]` attributes
- Requires validation scripts

### 6.7 Exercise: Create Client Browse View

**Try creating `Views/Client/Index.cshtml` to display available books**

Requirements:
- Show books in cards (Bootstrap)
- Display title, author, category
- Show "Book Now" button
- Use @model IEnumerable<Book>

<details>
<summary>Click to see solution</summary>

```razor
@model IEnumerable<LibraryWebApp.Models.Book>

@{
    ViewData["Title"] = "Browse Books";
}

<h1>Available Books</h1>

<div class="row">
    @foreach (var book in Model)
    {
        <div class="col-md-4 mb-4">
            <div class="card h-100">
                <div class="card-body">
                    <h5 class="card-title">@book.Title</h5>
                    <h6 class="card-subtitle mb-2 text-muted">
                        by @book.Author
                    </h6>
                    <p class="card-text">
                        <strong>Category:</strong> @book.Category<br />
                        <strong>Available:</strong> @book.AvailableCopies
                    </p>
                    @if (!string.IsNullOrEmpty(book.Description))
                    {
                        <p class="card-text">
                            @(book.Description.Length > 100 
                                ? book.Description.Substring(0, 100) + "..." 
                                : book.Description)
                        </p>
                    }
                </div>
                <div class="card-footer">
                    <a asp-action="BookDetails" asp-route-id="@book.Id" 
                       class="btn btn-sm btn-info">
                        ℹ️ Details
                    </a>
                    @if (book.AvailableCopies > 0)
                    {
                        <form method="post" asp-action="BookABook" 
                              asp-route-id="@book.Id" style="display:inline;">
                            <button type="submit" class="btn btn-sm btn-success">
                                📖 Book Now
                            </button>
                        </form>
                    }
                    else
                    {
                        <button class="btn btn-sm btn-secondary" disabled>
                            ❌ Unavailable
                        </button>
                    }
                </div>
            </div>
        </div>
    }
</div>
```

</details>

---

## Step 7: Authentication & Authorization

### 7.1 Understanding Sessions

**Sessions** store user data on the server between requests.

**Why Sessions?**
- Remember logged-in user
- Store user role (Admin/Client)
- Maintain state across pages

**Setting Session Data:**
```csharp
HttpContext.Session.SetString("Username", "john");
HttpContext.Session.SetInt32("UserId", 123);
```

**Getting Session Data:**
```csharp
var username = HttpContext.Session.GetString("Username");
var userId = HttpContext.Session.GetInt32("UserId");
```

**Clearing Session (Logout):**
```csharp
HttpContext.Session.Clear();
```

### 7.2 Implementing Authorization

**Pattern: Check Role in Every Action**

```csharp
public IActionResult AdminAction()
{
    // Check if user is admin
    var role = HttpContext.Session.GetString("Role");
    if (role != "Admin")
    {
        return RedirectToAction("Login", "Account");
    }
    
    // Continue with admin logic
    // ...
}
```

**Better Pattern: Helper Method**

```csharp
public class AdminController : Controller
{
    private bool IsAdmin()
    {
        var role = HttpContext.Session.GetString("Role");
        return role == "Admin";
    }
    
    public IActionResult AdminAction()
    {
        if (!IsAdmin()) 
            return RedirectToAction("Login", "Account");
            
        // Admin logic
    }
}
```

### 7.3 Role-Based Navigation

In `_Layout.cshtml`:

```razor
@if (Context.Session.GetString("Role") == "Admin")
{
    <li class="nav-item">
        <a class="nav-link" asp-controller="Admin" asp-action="Index">
            Admin Dashboard
        </a>
    </li>
}

@if (Context.Session.GetString("Role") == "Client")
{
    <li class="nav-item">
        <a class="nav-link" asp-controller="Client" asp-action="Browse">
            Browse Books
        </a>
    </li>
}
```

### 7.4 Password Security (Important!)

**⚠️ Current Implementation (UNSAFE for production):**
```csharp
user.Password = password;  // Plain text!
```

**✅ Production Implementation (SECURE):**
```csharp
using Microsoft.AspNetCore.Identity;

// Hash password when creating user
var hasher = new PasswordHasher<User>();
user.Password = hasher.HashPassword(user, password);

// Verify password when logging in
var result = hasher.VerifyHashedPassword(user, user.Password, password);
if (result == PasswordVerificationResult.Success)
{
    // Login successful
}
```

**For Learning:**
- Plain text is fine for now
- **NEVER** use in production
- Always hash passwords in real applications

---

## Step 8: Testing Your Application

### 8.1 Testing Checklist

**1. Start MongoDB:**
```bash
sudo systemctl start mongod
sudo systemctl status mongod
```

**2. Initialize Database:**
```bash
cd /home/ergo/Desktop/dot-net/LibraryWebApp
./init-database.sh
```

**3. Run Application:**
```bash
dotnet run
```

**4. Test Cases:**

✅ **Authentication:**
- [ ] Login as admin (admin/admin123)
- [ ] Login as client (client1/client123)
- [ ] Try invalid credentials
- [ ] Logout

✅ **Admin Features:**
- [ ] View dashboard statistics
- [ ] Create a category
- [ ] Add a new book
- [ ] Edit a book
- [ ] Delete a book
- [ ] View all bookings
- [ ] Approve a booking
- [ ] View all clients

✅ **Client Features:**
- [ ] Register new account
- [ ] Browse available books
- [ ] Search for books
- [ ] View book details
- [ ] Book a book
- [ ] View my bookings
- [ ] Cancel pending booking

### 8.2 Common Issues & Solutions

**Issue: Can't connect to MongoDB**
```bash
# Solution: Start MongoDB
sudo systemctl start mongod
```

**Issue: Port already in use**
```bash
# Solution: Kill process or change port
sudo lsof -i :5284
sudo kill -9 <PID>
```

**Issue: Session not working**
```csharp
// Solution: Make sure session is configured
builder.Services.AddSession();
app.UseSession();  // Must be before UseRouting()
```

**Issue: Model validation not working**
```razor
<!-- Solution: Add validation scripts -->
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

## Key Concepts Explained

### MVC Architecture

**Why MVC?**
- **Separation of Concerns** - Each part has one job
- **Maintainability** - Easy to find and fix bugs
- **Testability** - Can test parts independently
- **Reusability** - Services can be used by multiple controllers

**Example:**
```
User wants to see book list:
1. Browser → GET /Admin/Books
2. Routing → AdminController.Books()
3. Controller → BookService.GetAllAsync()
4. Service → MongoDB
5. MongoDB → Returns books
6. Service → Returns List<Book>
7. Controller → return View(books)
8. View → Renders HTML with books
9. HTML → Browser
```

### Dependency Injection

**What is it?**
Instead of creating dependencies yourself:
```csharp
// BAD: Creating dependency manually
public class MyController
{
    private BookService _bookService = new BookService();
}
```

Let the framework inject them:
```csharp
// GOOD: Dependency injection
public class MyController
{
    private readonly BookService _bookService;
    
    public MyController(BookService bookService)
    {
        _bookService = bookService;
    }
}
```

**Benefits:**
- Loose coupling
- Easy testing (can inject mocks)
- Single instance management
- Easier to change implementations

### Async/Await

**Why async?**
```csharp
// Synchronous (blocks thread)
var books = GetBooks();  // Waits here, thread blocked

// Asynchronous (non-blocking)
var books = await GetBooksAsync();  // Thread can do other work
```

**When to use async:**
- Database operations
- File operations
- HTTP requests
- Any I/O operation

**Pattern:**
```csharp
public async Task<List<Book>> GetBooksAsync()
{
    return await _collection.Find(_ => true).ToListAsync();
}
```

### Routing

**Convention-Based Routing:**
```csharp
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

**URL Examples:**
- `/` → HomeController.Index()
- `/Admin` → AdminController.Index()
- `/Admin/Books` → AdminController.Books()
- `/Admin/EditBook/123` → AdminController.EditBook("123")

**Route Parameters:**
```csharp
// URL: /Admin/EditBook/123
public IActionResult EditBook(string id)
{
    // id = "123"
}
```

---

## Common Patterns

### CRUD Pattern

Every entity follows this pattern:

```csharp
// CREATE
public async Task CreateAsync(Book book)
{
    await _collection.InsertOneAsync(book);
}

// READ (Get All)
public async Task<List<Book>> GetAllAsync()
{
    return await _collection.Find(_ => true).ToListAsync();
}

// READ (Get By ID)
public async Task<Book?> GetByIdAsync(string id)
{
    return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
}

// UPDATE
public async Task UpdateAsync(string id, Book book)
{
    await _collection.ReplaceOneAsync(x => x.Id == id, book);
}

// DELETE
public async Task DeleteAsync(string id)
{
    await _collection.DeleteOneAsync(x => x.Id == id);
}
```

### Controller-Service Pattern

```csharp
// Controller: Handles HTTP, calls service
public class BookController : Controller
{
    private readonly BookService _bookService;
    
    public BookController(BookService bookService)
    {
        _bookService = bookService;
    }
    
    public async Task<IActionResult> Index()
    {
        var books = await _bookService.GetAllAsync();
        return View(books);
    }
}

// Service: Handles business logic, database
public class BookService
{
    private readonly IMongoCollection<Book> _books;
    
    public async Task<List<Book>> GetAllAsync()
    {
        return await _books.Find(_ => true).ToListAsync();
    }
}
```

### ViewBag Pattern

**Pass data to view:**
```csharp
// Controller
ViewBag.Message = "Hello!";
ViewBag.Count = 10;
ViewBag.Books = bookList;

// View
<p>@ViewBag.Message</p>
<p>Count: @ViewBag.Count</p>
@foreach (var book in ViewBag.Books)
{
    <p>@book.Title</p>
}
```

### Model Binding Pattern

```csharp
// Form in View
<form method="post">
    <input name="Title" />
    <input name="Author" />
    <button type="submit">Submit</button>
</form>

// Controller receives data
[HttpPost]
public IActionResult Create(string Title, string Author)
{
    // Title and Author automatically filled from form
}

// OR bind to model
[HttpPost]
public IActionResult Create(Book book)
{
    // book.Title and book.Author automatically filled
}
```

---

## Next Steps

### 1. Enhance Your Application

**Add Features:**
- [ ] Book reviews and ratings
- [ ] Advanced search filters
- [ ] Book cover images
- [ ] Email notifications
- [ ] Export to PDF/Excel
- [ ] Book reservations
- [ ] Fine payment system

### 2. Improve Security

- [ ] Hash passwords (BCrypt)
- [ ] Add HTTPS enforcement
- [ ] Implement CAPTCHA
- [ ] Add rate limiting
- [ ] Input sanitization
- [ ] SQL injection protection (MongoDB already safe)

### 3. Learn More

**Topics to Study:**
- [ ] Entity Framework Core (alternative to MongoDB)
- [ ] ASP.NET Core Identity (advanced authentication)
- [ ] SignalR (real-time updates)
- [ ] Web APIs (RESTful services)
- [ ] Blazor (C# in the browser)
- [ ] Docker (containerization)
- [ ] Azure/AWS deployment

**Recommended Resources:**
- Microsoft Docs: https://docs.microsoft.com/aspnet/core
- MongoDB University: https://university.mongodb.com
- C# Yellow Book (free): http://www.csharpcourse.com
- Pluralsight: ASP.NET Core Path
- YouTube: kudvenkat, IAmTimCorey

### 4. Practice Projects

**Build these to practice:**
- [ ] Blog system
- [ ] E-commerce store
- [ ] Task management app
- [ ] Social media platform
- [ ] CRM system
- [ ] Inventory management

---

## Congratulations! 🎉

You've learned:
- ✅ ASP.NET Core MVC architecture
- ✅ MongoDB integration
- ✅ CRUD operations
- ✅ Authentication & authorization
- ✅ Routing and navigation
- ✅ Razor views
- ✅ Dependency injection
- ✅ Async programming

You can now build full-stack web applications with .NET!

---

**Need Help?**
- Check official docs
- Ask on Stack Overflow
- Join .NET Discord communities
- Review the working code in this project

**Keep Learning!** 🚀

The best way to learn is by building. Take this project, modify it, break it, fix it, and make it your own!
