# 📚 Library Management System (Lumen Library)

A modern, full-featured library management web application built with **ASP.NET Core 8.0 MVC**. This system provides a complete solution for managing library operations including book cataloging, user management, and a sophisticated reservation workflow with multi-state approval process.

![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/C%23-12-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Bootstrap](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)

## ✨ Key Highlights

- 🎯 **Production-Ready Architecture**: Clean MVC pattern with layered design
- 🔐 **Secure Authentication**: Session-based auth with role-based access control (RBAC)
- 📖 **Rich Catalog**: Pre-loaded with 50 books across 6 categories with real cover images
- 🔄 **Advanced Workflow**: Multi-state booking process (Pending → Approved/Refused → Completed)
- 📱 **Responsive Design**: Modern UI built with Bootstrap 5, works on all devices
- ⚡ **High Performance**: In-memory storage with thread-safe operations
- 📊 **Admin Dashboard**: Comprehensive statistics and management tools
- 🎨 **Beautiful UX**: Card-based layout with smooth animations and visual feedback

---

## 🌟 Features

### 👥 For Library Members (Clients)

| Feature | Description |
|---------|-------------|
| 📚 **Browse Catalog** | Explore books organized by 6 categories with stunning cover images |
| 🔍 **Smart Search** | Find books instantly by title or author (case-insensitive) |
| 📖 **Book Details** | View complete information: ISBN, description, publication year, author bio |
| 🎫 **One-Click Reservation** | Reserve available books with instant confirmation |
| 📊 **Track Bookings** | Monitor reservation status with organized tabs (Pending, Approved, History) |
| ⏰ **Due Date Tracking** | See due dates for approved reservations |
| 🎨 **Visual Status** | Color-coded badges for booking states |

### 👨‍💼 For Administrators

| Feature | Description |
|---------|-------------|
| 📊 **Dashboard** | Real-time statistics: total books, categories, active bookings, members |
| 📚 **Book Management** | Full CRUD operations with cover image upload support |
| 🏷️ **Category Management** | Create and manage book categories with icons |
| ✅ **Booking Approval** | Approve or refuse reservations with custom due date assignment |
| 📜 **Complete History** | View all bookings with filters (status, search) and overdue alerts |
| 👥 **Member Management** | View registered users with registration dates |
| 🔴 **Overdue Detection** | Automatic highlighting of late returns with visual alerts |
| 🔄 **Status Management** | Mark books as returned and complete the booking cycle |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher

### Quick Start

```bash
# 1. Clone the repository
git clone https://github.com/Oussama-Ouaanine/DotNet.git
cd DotNet/LibraryWebApp

# 2. Build the project
dotnet build

# 3. Run the application
dotnet run

# 4. Open your browser
# Navigate to: http://localhost:5284
```

The application will start with:
- ✅ 50 pre-loaded books with real cover images
- ✅ 6 categories (Fiction, Business, Technology, Wellness, Biography, History)
- ✅ 3 test user accounts ready to use

---

## 🔐 Test Accounts

Use these pre-configured accounts to explore the system:

| Role | Email | Password | Capabilities |
|------|-------|----------|--------------|
| 🔑 **Admin** | admin@lumenlibrary.com | admin123 | Full system access, manage books, approve bookings |
| 👤 **Client** | maya@readers.com | reader123 | Browse catalog, reserve books, track bookings |
| 👤 **Client** | leo@readers.com | reader123 | Browse catalog, reserve books, track bookings |

---

## 📚 Pre-loaded Book Catalog

The system includes **50 carefully selected books** across 6 categories:

### 📖 Fiction (10 books)
- The Great Gatsby, 1984, To Kill a Mockingbird, Pride and Prejudice, The Catcher in the Rye, Lord of the Flies, Animal Farm, Brave New World, Jane Eyre, Wuthering Heights

### 💼 Business (8 books)
- Think and Grow Rich, The Lean Startup, Good to Great, Zero to One, The 4-Hour Workweek, The E-Myth Revisited, Start with Why, The Hard Thing About Hard Things

### 💻 Technology (7 books)
- Clean Code, The Pragmatic Programmer, Design Patterns, You Don't Know JS, The Phoenix Project, Cracking the Coding Interview, Code Complete

### 🧘 Wellness (7 books)
- Atomic Habits, The Power of Now, How to Win Friends and Influence People, The 7 Habits of Highly Effective People, Mindset, Grit, The Subtle Art of Not Giving a F*ck

### 👤 Biography (6 books)
- Steve Jobs, Becoming, Long Walk to Freedom, The Diary of a Young Girl, Educated, Born a Crime

### 🌍 History (6 books)
- Sapiens, Guns Germs and Steel, The Silk Roads, A People's History of the United States, SPQR, 1776

*All books feature real cover images hosted on Amazon for an authentic library experience.*

---

## 🔄 Booking Workflow

The system implements a sophisticated multi-state reservation process:

```
┌─────────────┐
│   Client    │
│  Reserves   │──────► Status: PENDING (Yellow)
│    Book     │
└─────────────┘
       │
       ▼
┌─────────────────────────────┐
│  Admin Reviews Request      │
└─────────────────────────────┘
       │
       ├──── Approve ────► Status: APPROVED (Green)
       │                  + Due Date Assigned
       │                  + Client can borrow book
       │
       └──── Refuse ─────► Status: REFUSED (Red)
                          + Request declined

       │ (After borrowing period)
       ▼
┌─────────────────────────────┐
│  Admin Marks as Returned    │──────► Status: COMPLETED (Blue)
└─────────────────────────────┘        + Moves to History
```

**Features:**
- 📅 Custom due date assignment on approval
- 🔴 Automatic overdue detection with visual alerts
- 📊 Organized tracking (Pending, Approved, History tabs)
- 🔔 Visual status indicators with color coding
- ⏰ Timestamp tracking (requested, approved, returned dates)

---

## 🏗️ Architecture & Design

### Project Structure

```
LibraryWebApp/
├── 📁 Controllers/              # MVC Controllers
│   ├── HomeController.cs        # Public landing page
│   ├── AccountController.cs     # Login/Register/Logout
│   ├── ClientController.cs      # Member portal (Browse, Reserve, MyBookings)
│   └── AdminController.cs       # Admin portal (Dashboard, CRUD operations)
│
├── 📁 Models/                   # Domain Entities
│   ├── User.cs                  # User entity with role
│   ├── Book.cs                  # Book entity with metadata
│   ├── Category.cs              # Category entity
│   ├── Booking.cs               # Booking entity with status
│   ├── BookingStatus.cs         # Enum (Pending, Approved, Refused, Completed)
│   └── 📁 ViewModels/           # View-specific models
│       ├── Account/             # Login/Register models
│       ├── Admin/               # Admin dashboard models
│       └── Client/              # Client portal models
│
├── 📁 Services/                 # Business Logic Layer
│   ├── UserService.cs           # User management (register, auth)
│   ├── BookService.cs           # Book CRUD + search
│   ├── CategoryService.cs       # Category management
│   └── BookingService.cs        # Booking workflow
│
├── 📁 Views/                    # Razor Views
│   ├── 📁 Home/                 # Public pages
│   ├── 📁 Account/              # Auth pages
│   ├── 📁 Client/               # Member interface
│   ├── 📁 Admin/                # Admin interface
│   └── 📁 Shared/               # Layouts & partials
│
├── 📁 wwwroot/                  # Static Resources
│   ├── 📁 css/                  # Custom styles
│   ├── 📁 js/                   # JavaScript
│   ├── 📁 lib/                  # Bootstrap, jQuery
│   └── 📁 uploads/books/        # Book cover images
│
└── Program.cs                   # App configuration & startup
```

### Layered Architecture

```
┌─────────────────────────────────────────┐
│     Presentation Layer                  │
│  (Controllers + Razor Views)            │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│     Business Logic Layer                │
│  (Services: Book, User, Booking, etc.)  │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│     Data Access Layer                   │
│  (In-Memory Storage with Thread-Safety) │
└─────────────────────────────────────────┘
                  ↓
┌─────────────────────────────────────────┐
│     Domain Layer                        │
│  (Models: Book, User, Booking, etc.)    │
└─────────────────────────────────────────┘
```

### Design Patterns Used

| Pattern | Implementation | Purpose |
|---------|---------------|---------|
| **MVC** | ASP.NET Core MVC | Separation of concerns |
| **Singleton** | Service registration | Single shared instance per service |
| **Repository** | Services act as repositories | Data access abstraction |
| **ViewModel** | Dedicated view models | Decoupling domain from presentation |
| **Dependency Injection** | Built-in DI container | Loose coupling & testability |
| **Session State** | ASP.NET Core Sessions | Authentication & user context |

---

## 🔒 Security Features

- ✅ **Session-based Authentication**: 30-minute idle timeout with secure HttpOnly cookies
- ✅ **Role-based Authorization**: Separate access levels (Admin/Client) with route protection
- ✅ **CSRF Protection**: Anti-forgery tokens on all POST forms
- ✅ **Input Validation**: Server-side validation with Data Annotations
- ✅ **Thread Safety**: Mutex locks on all shared data operations
- ✅ **XSS Prevention**: Razor automatic HTML encoding
- ✅ **Session Security**: Secure cookie configuration with SameSite policy

---

## 💻 Technology Stack

| Category | Technology | Version |
|----------|-----------|---------|
| **Framework** | ASP.NET Core MVC | 8.0 |
| **Language** | C# | 12 |
| **Template Engine** | Razor Pages | - |
| **Frontend Framework** | Bootstrap | 5.3 |
| **JavaScript Library** | jQuery | 3.x |
| **Validation** | jQuery Validation | - |
| **Storage** | In-Memory (List<T>) | - |
| **Authentication** | ASP.NET Sessions | - |

---

## 🎨 UI/UX Features

- 📱 **Fully Responsive**: Mobile-first design, works on phones, tablets, and desktops
- 🎨 **Modern Card Layout**: Book grid with hover effects and shadows
- 🌈 **Visual Status Indicators**: Color-coded badges (Yellow=Pending, Green=Approved, Red=Refused, Blue=Completed)
- 🔍 **Instant Search**: Real-time filtering without page reload
- 🎯 **Smart Navigation**: Breadcrumbs, organized tabs, clear CTAs
- ✨ **Smooth Animations**: Transitions on hover and state changes
- 🔔 **Toast Notifications**: Success/error messages with TempData
- ⚠️ **Overdue Alerts**: Red badges for late returns
- 🖼️ **Image Optimization**: Lazy loading for book covers
- 🎨 **Consistent Theming**: Professional color scheme throughout

---

## 📖 Documentation

### Academic Report (French)

Complete technical documentation available in `latexrappotr.md` (1100+ lines LaTeX):

- 📐 **UML Diagrams**: Use Case, Class, Sequence diagrams (UML 2.5 compliant)
- 🏗️ **Architecture**: Detailed explanation of MVC pattern and layered design
- 💻 **Implementation**: Code examples and technical decisions
- 🧪 **Testing**: Test scenarios and validation procedures
- 🔮 **Future Work**: Planned enhancements and improvements
- 📚 **Bibliography**: 18 academic and technical references

**Generate PDF:**
```bash
pdflatex latexrappotr.md
pdflatex latexrappotr.md  # Run twice for TOC
```

---

## 🚀 Future Enhancements

### Phase 1: Production Readiness
- [ ] Database persistence (MongoDB or SQL Server)
- [ ] Entity Framework Core integration
- [ ] Password hashing (BCrypt/Argon2)
- [ ] ASP.NET Core Identity for authentication
- [ ] Logging with Serilog

### Phase 2: Advanced Features
- [ ] Email notifications (booking approval, due date reminders)
- [ ] Fine system for overdue books
- [ ] Book recommendation engine
- [ ] Advanced statistics dashboard with charts (Chart.js)
- [ ] PDF/Excel report generation

### Phase 3: API & Mobile
- [ ] REST API for mobile apps
- [ ] JWT authentication
- [ ] Swagger/OpenAPI documentation
- [ ] OAuth 2.0 social login

### Phase 4: UX Improvements
- [ ] Multi-language support (i18n)
- [ ] Dark mode theme
- [ ] Book rating and reviews
- [ ] Reading lists and favorites
- [ ] Real-time notifications (SignalR)

---

## 🧪 Testing

### Manual Test Scenarios

1. **Authentication Flow**
   - Register new account
   - Login with valid credentials
   - Login with invalid credentials
   - Session timeout after 30 minutes
   - Role-based access control

2. **Booking Workflow**
   - Client reserves available book
   - Admin approves with due date
   - Admin refuses reservation
   - Admin marks book as returned
   - Overdue detection

3. **Book Management**
   - Create book with image upload
   - Edit book details
   - Delete book
   - Category assignment

4. **Search & Browse**
   - Search by title
   - Search by author
   - Filter by category
   - Combine search + filter

---

## 📝 Contributing

This is an academic project for EMSI. Contributions, issues, and feature requests are welcome!

---

## 📄 License

This project is developed as part of an academic assignment for **EMSI** (École Marocaine Des Sciences De L'Ingénieur).

**Academic Year**: 2024-2025  
**Institution**: EMSI Morocco  
**Course**: Software Engineering / Web Development

---

## 👨‍💻 Author

**Oussama Ouaanine**  
📧 Email: [Contact via GitHub](https://github.com/Oussama-Ouaanine)  
🎓 EMSI Student - Software Engineering  
📅 Academic Year: 2024/2025

---

## 🙏 Acknowledgments

- **ASP.NET Core Team** for the excellent framework
- **Bootstrap Team** for the responsive CSS framework
- **EMSI** for the academic guidance and support
- **Open Source Community** for inspiration and resources

---

<div align="center">

**⭐ If you find this project useful, please consider giving it a star! ⭐**

Made with ❤️ using ASP.NET Core 8.0

[Report Bug](https://github.com/Oussama-Ouaanine/DotNet/issues) · [Request Feature](https://github.com/Oussama-Ouaanine/DotNet/issues)

</div>
