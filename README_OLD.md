# 📚 Library Management System

A modern web-based library management system built with ASP.NET Core MVC, featuring a role-based access control system for administrators and members.

## 🌟 Features

### For Members (Clients)
- 📖 Browse books by category with grid layout
- 🔍 Search books by title or author
- 📝 Reserve books online
- 📊 Track reservation status (Pending, Approved, History)
- 🎨 Modern, responsive UI with book cover images

### For Administrators
- 📈 Dashboard with statistics and insights
- ➕ Manage books (Create, Edit, Delete) with cover image upload
- 🏷️ Manage categories
- ✅ Approve/Refuse booking requests with due dates
- 📅 Track overdue bookings
- 👥 View registered members
- 🔍 Advanced filtering and search in booking history

## 🛠️ Technologies

- **Backend**: ASP.NET Core 8.0 MVC
- **Language**: C# 12
- **Frontend**: Razor Pages, Bootstrap 5.3, jQuery
- **Data Storage**: In-memory (singleton services with thread-safe locks)
- **Authentication**: Session-based (30-minute timeout)

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- A modern web browser

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Oussama-Ouaanine/DotNet.git
   cd DotNet
   ```

2. **Navigate to the project directory**
   ```bash
   cd LibraryWebApp
   ```

3. **Restore dependencies**
   ```bash
   dotnet restore
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Access the application**
   - Open your browser and navigate to: `http://localhost:5284`

## 👤 Test Accounts

### Administrator
- **Email**: admin@lumenlibrary.com
- **Password**: admin123

### Members
- **Email**: maya@readers.com | **Password**: reader123
- **Email**: leo@readers.com | **Password**: reader123

## 📖 Catalog

The system comes pre-loaded with **50 books** across 6 categories:
- Fiction (10 books)
- Business (8 books)
- Technology (7 books)
- Wellbeing (7 books)
- Biography (6 books)
- History (6 books)

Each book features a real cover image for enhanced visual experience.

## 🔄 Booking Workflow

```
1. Client reserves a book → Status: Pending
2. Admin reviews the request:
   - Approve with due date → Status: Approved
   - Refuse → Status: Refused
3. Admin marks book as returned → Status: Completed
```

## 📁 Project Structure

```
LibraryWebApp/
├── Controllers/          # MVC Controllers (Home, Account, Client, Admin)
├── Models/              # Domain entities and ViewModels
├── Services/            # Business logic layer (singleton services)
├── Views/               # Razor view templates
├── wwwroot/             # Static files (CSS, JS, images)
├── Program.cs           # Application entry point
└── appsettings.json     # Configuration
```

## 🏗️ Architecture

The application follows the **MVC (Model-View-Controller)** pattern with a clear separation of concerns:

- **Controllers**: Handle HTTP requests and coordinate between services and views
- **Services**: Contain business logic and data management (UserService, BookService, BookingService, CategoryService)
- **Models**: Define domain entities (User, Book, Booking, Category)
- **Views**: Razor templates for rendering HTML

### Key Design Patterns
- **Singleton**: Services are registered as singletons for shared state
- **Dependency Injection**: ASP.NET Core's built-in DI container
- **Repository Pattern**: Services act as in-memory repositories
- **Thread Safety**: Mutex locks protect shared collections

## 🔐 Security Features

- Session-based authentication with 30-minute timeout
- Role-based access control (Admin vs Client)
- Anti-CSRF token validation on forms
- HttpOnly and Essential cookies
- Authorization guards on protected routes

## 🎨 UI/UX Highlights

- **Responsive Design**: Bootstrap 5.3 grid system
- **Modern Aesthetics**: Apple-inspired design language
- **Visual Feedback**: Toast notifications, status badges, hover effects
- **Accessibility**: Semantic HTML, proper form labels

## 📄 Documentation

- **Academic Report**: See `latexrappotr.md` for comprehensive project documentation including UML diagrams (Use Case, Class, Sequence)
- **Compile Report**:
  ```bash
  pdflatex latexrappotr.md
  pdflatex latexrappotr.md  # Run twice for table of contents
  ```

## 🚀 Future Enhancements

- Database persistence (SQL Server, PostgreSQL, or MongoDB)
- Password hashing (BCrypt/Argon2)
- Email notifications
- Advanced statistics dashboard
- REST API for mobile apps
- Fine calculation for overdue books
- Book recommendation system

## 📝 License

This project is part of an academic assignment for EMSI (École Marocaine Des Sciences De L'Ingénieur).

## 👨‍💻 Author

**Oussama Ouaanine**
- GitHub: [@Oussama-Ouaanine](https://github.com/Oussama-Ouaanine)

## 🙏 Acknowledgments

- ASP.NET Core documentation
- Bootstrap team
- EMSI for the learning opportunity

---

*"A library is a freedom." — Ursula K. Le Guin*
