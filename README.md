# Library Web Application

An Online Library Management System built with ASP.NET Core MVC and MongoDB.

## Features

### For Clients
- ✅ User Registration and Login
- ✅ Browse and Search Books
- ✅ Book Reservation System
- ✅ View Booking History
- ✅ Track Booking Status
- ✅ View Book Details

### For Administrators
- ✅ Dashboard with Statistics
- ✅ Manage Books (Add, Edit, Delete)
- ✅ Manage Categories
- ✅ Approve/Decline Booking Requests
- ✅ View All Bookings
- ✅ View Registered Clients
- ✅ Manage Book Availability

## Technologies Used

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: MongoDB
- **Language**: C#
- **Frontend**: Bootstrap 5, Razor Views
- **Driver**: MongoDB.Driver 3.5.0

## Prerequisites

- .NET SDK 8.0 or higher
- MongoDB Server (local or cloud)
- Visual Studio Code (or any IDE)

## Installation & Setup

### 1. Install MongoDB

**Option A: Install Locally**
```bash
# Ubuntu/Debian
sudo apt-get install -y mongodb-org

# Start MongoDB service
sudo systemctl start mongod
sudo systemctl enable mongod
```

**Option B: Use MongoDB Atlas (Cloud)**
- Sign up at https://www.mongodb.com/cloud/atlas
- Create a free cluster
- Get your connection string

### 2. Configure Database Connection

Edit `appsettings.json`:
```json
{
  "DatabaseSettings": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "LibraryDB"
  }
}
```

For MongoDB Atlas, use your connection string:
```json
{
  "DatabaseSettings": {
    "ConnectionString": "mongodb+srv://<username>:<password>@cluster.mongodb.net/",
    "DatabaseName": "LibraryDB"
  }
}
```

### 3. Build and Run

```bash
cd LibraryWebApp
dotnet restore
dotnet build
dotnet run
```

The application will start at:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

## Default Users

Create an admin user manually in MongoDB or through the registration page and change the role to "Admin".

**To create an admin via MongoDB shell:**
```javascript
use LibraryDB
db.Users.insertOne({
    userId: 1,
    username: "admin",
    password: "admin123",
    email: "admin@library.com",
    role: "Admin",
    registrationDate: new Date(),
    adminLevel: "Super"
})
```

## Project Structure

```
LibraryWebApp/
├── Controllers/
│   ├── AccountController.cs    # Authentication
│   ├── AdminController.cs      # Admin operations
│   ├── ClientController.cs     # Client operations
│   └── HomeController.cs       # Home page
├── Models/
│   ├── User.cs                 # User model
│   ├── Book.cs                 # Book model
│   ├── Booking.cs              # Booking model
│   ├── Category.cs             # Category model
│   └── DatabaseSettings.cs     # MongoDB settings
├── Services/
│   ├── MongoDbService.cs       # MongoDB connection
│   ├── UserService.cs          # User operations
│   ├── BookService.cs          # Book operations
│   ├── BookingService.cs       # Booking operations
│   └── CategoryService.cs      # Category operations
├── Views/
│   ├── Account/                # Login, Register
│   ├── Admin/                  # Admin views
│   ├── Client/                 # Client views
│   ├── Home/                   # Home page
│   └── Shared/                 # Layout, shared views
└── Program.cs                  # Application entry point
```

## Usage Guide

### For Clients:
1. Register an account
2. Login with your credentials
3. Browse or search for books
4. Click "Book Now" to request a book
5. View your bookings in "My Bookings"
6. Wait for admin approval

### For Administrators:
1. Login with admin credentials
2. Access the admin dashboard
3. Add/Edit/Delete books and categories
4. Review pending booking requests
5. Approve or decline bookings
6. View all clients and bookings

## UML Diagrams

The system design is based on:
- **Class Diagram**: Located in `/PJ/CLASS DIAGRAM.png`
- **Use Case Diagram**: Located in `/PJ/USE_CASE_DIAGRAM.png`

## API Endpoints

### Account
- `GET/POST /Account/Login` - User login
- `GET/POST /Account/Register` - User registration
- `GET /Account/Logout` - User logout

### Client
- `GET /Client/Index` - View available books
- `GET /Client/Browse` - Browse all books
- `GET /Client/Search` - Search books
- `POST /Client/BookABook/{id}` - Book a book
- `GET /Client/MyBookings` - View my bookings

### Admin
- `GET /Admin/Index` - Admin dashboard
- `GET /Admin/Books` - Manage books
- `GET/POST /Admin/CreateBook` - Add new book
- `GET/POST /Admin/EditBook/{id}` - Edit book
- `POST /Admin/DeleteBook/{id}` - Delete book
- `GET /Admin/Bookings` - View all bookings
- `POST /Admin/ApproveBooking/{id}` - Approve booking
- `POST /Admin/DeclineBooking/{id}` - Decline booking

## Troubleshooting

### MongoDB Connection Issues
```bash
# Check if MongoDB is running
sudo systemctl status mongod

# Check MongoDB logs
sudo tail -f /var/log/mongodb/mongod.log
```

### Port Already in Use
```bash
# Change port in Properties/launchSettings.json
# Or kill process using the port
sudo lsof -i :5000
sudo kill -9 <PID>
```

## Future Enhancements

- [ ] Email notifications for booking approvals
- [ ] Book reviews and ratings
- [ ] Advanced search filters
- [ ] Book recommendations
- [ ] Return book functionality for clients
- [ ] Fine payment system
- [ ] Report generation

## License

This project is created for educational purposes.

## Author

Library Management System
Date: October 2025
