#!/bin/bash

echo "=============================================="
echo "Library Web App - Database Initialization"
echo "=============================================="

# Connect to MongoDB and create initial data
mongosh << 'EOF'

use LibraryDB

// Create an admin user
db.Users.insertOne({
    userId: 1,
    username: "admin",
    password: "admin123",
    email: "admin@library.com",
    role: "Admin",
    registrationDate: new Date(),
    adminLevel: "Super"
})

// Create a test client
db.Users.insertOne({
    userId: 2,
    username: "client1",
    password: "client123",
    email: "client@example.com",
    role: "Client",
    registrationDate: new Date(),
    phoneNumber: "123-456-7890",
    address: "123 Main St, City"
})

// Create categories
db.Categories.insertMany([
    { categoryId: 1, categoryName: "Fiction", description: "Fiction books including novels and short stories" },
    { categoryId: 2, categoryName: "Non-Fiction", description: "Non-fiction books based on real events and facts" },
    { categoryId: 3, categoryName: "Science", description: "Scientific books and research" },
    { categoryId: 4, categoryName: "Technology", description: "Technology and computer science books" },
    { categoryId: 5, categoryName: "History", description: "Historical books and biographies" },
    { categoryId: 6, categoryName: "Biography", description: "Life stories and memoirs" }
])

// Create sample books
db.Books.insertMany([
    {
        bookId: 1,
        title: "The Great Gatsby",
        author: "F. Scott Fitzgerald",
        category: "Fiction",
        ISBN: "978-0-7432-7356-5",
        publicationYear: 1925,
        description: "A classic American novel set in the Jazz Age",
        status: "Available",
        availableCopies: 5,
        totalCopies: 5
    },
    {
        bookId: 2,
        title: "Clean Code",
        author: "Robert C. Martin",
        category: "Technology",
        ISBN: "978-0-13-235088-4",
        publicationYear: 2008,
        description: "A handbook of agile software craftsmanship",
        status: "Available",
        availableCopies: 3,
        totalCopies: 3
    },
    {
        bookId: 3,
        title: "Sapiens",
        author: "Yuval Noah Harari",
        category: "History",
        ISBN: "978-0-06-231609-7",
        publicationYear: 2011,
        description: "A brief history of humankind",
        status: "Available",
        availableCopies: 4,
        totalCopies: 4
    },
    {
        bookId: 4,
        title: "1984",
        author: "George Orwell",
        category: "Fiction",
        ISBN: "978-0-452-28423-4",
        publicationYear: 1949,
        description: "A dystopian social science fiction novel",
        status: "Available",
        availableCopies: 6,
        totalCopies: 6
    },
    {
        bookId: 5,
        title: "A Brief History of Time",
        author: "Stephen Hawking",
        category: "Science",
        ISBN: "978-0-553-38016-3",
        publicationYear: 1988,
        description: "Cosmology explained for non-scientists",
        status: "Available",
        availableCopies: 2,
        totalCopies: 2
    }
])

print("Database initialized successfully!")
print("Admin credentials: username='admin', password='admin123'")
print("Client credentials: username='client1', password='client123'")

EOF

echo ""
echo "Database initialization complete!"
