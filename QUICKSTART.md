# Quick Start Guide

## Prerequisites Check

```bash
# Check .NET SDK
dotnet --version
# Should show 8.0.x or higher

# Check if MongoDB is installed
mongod --version
```

## Installation Steps

### Step 1: Install MongoDB (if not already installed)

```bash
# Navigate to project directory
cd /home/ergo/Desktop/dot-net/LibraryWebApp

# Run MongoDB installation script
sudo ./install-mongodb.sh
```

**OR use MongoDB Atlas (Cloud):**
1. Go to https://www.mongodb.com/cloud/atlas
2. Create a free account
3. Create a cluster
4. Get your connection string
5. Update `appsettings.json` with your connection string

### Step 2: Initialize Database with Sample Data

```bash
# Make sure MongoDB is running
sudo systemctl status mongod

# Run initialization script
./init-database.sh
```

This creates:
- Admin user: username=`admin`, password=`admin123`
- Client user: username=`client1`, password=`client123`
- Sample categories
- Sample books

### Step 3: Run the Application

```bash
# Navigate to project directory
cd /home/ergo/Desktop/dot-net/LibraryWebApp

# Build the project
dotnet build

# Run the application
dotnet run
```

The application will start at:
- **HTTPS**: https://localhost:5001
- **HTTP**: http://localhost:5000

### Step 4: Access the Application

Open your browser and navigate to: `https://localhost:5001`

## Test Accounts

### Admin Account
- **Username**: admin
- **Password**: admin123
- **Capabilities**: Manage books, categories, approve bookings, view clients

### Client Account
- **Username**: client1
- **Password**: client123
- **Capabilities**: Browse books, search, book books, view bookings

## Common Commands

### MongoDB Commands
```bash
# Start MongoDB
sudo systemctl start mongod

# Stop MongoDB
sudo systemctl stop mongod

# Restart MongoDB
sudo systemctl restart mongod

# Check MongoDB status
sudo systemctl status mongod

# Access MongoDB shell
mongosh

# Connect to LibraryDB
use LibraryDB

# View collections
show collections

# View all books
db.Books.find().pretty()

# View all users
db.Users.find().pretty()
```

### .NET Commands
```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run project
dotnet run

# Run with watch (auto-reload)
dotnet watch run

# Clean build artifacts
dotnet clean
```

## Troubleshooting

### Issue: MongoDB connection refused
**Solution:**
```bash
sudo systemctl start mongod
sudo systemctl status mongod
```

### Issue: Port 5000 or 5001 already in use
**Solution:**
```bash
# Find process using port
sudo lsof -i :5000

# Kill the process
sudo kill -9 <PID>

# Or change port in Properties/launchSettings.json
```

### Issue: Can't login with admin credentials
**Solution:**
```bash
# Re-run database initialization
./init-database.sh

# Or manually create admin in MongoDB
mongosh
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

### Issue: Package restore fails
**Solution:**
```bash
# Clear NuGet cache
dotnet nuget locals all --clear

# Restore packages
dotnet restore
```

## Development Tips

### Hot Reload (Auto-refresh on code changes)
```bash
dotnet watch run
```

### View Logs
The application logs will appear in the terminal where you ran `dotnet run`.

### Database GUI Tools
- **MongoDB Compass**: https://www.mongodb.com/products/compass
- **Studio 3T**: https://studio3t.com/
- **Robo 3T**: https://robomongo.org/

### VS Code Extensions (Recommended)
- C# Dev Kit (already installed)
- MongoDB for VS Code
- .NET Core Test Explorer
- GitLens

## Next Steps

1. Login as admin and create more categories
2. Add more books to the library
3. Create client accounts and test the booking flow
4. Customize the UI in Views folder
5. Add more features as needed

## Support

For issues or questions:
1. Check the README.md file
2. Review error messages in terminal
3. Check MongoDB logs: `sudo tail -f /var/log/mongodb/mongod.log`
4. Verify MongoDB is running: `sudo systemctl status mongod`

Happy coding! 🚀
