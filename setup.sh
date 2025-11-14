#!/bin/bash

echo "=================================================="
echo "  Library Web App - Complete Setup Script"
echo "=================================================="
echo ""

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[✓]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[!]${NC} $1"
}

print_error() {
    echo -e "${RED}[✗]${NC} $1"
}

# Check .NET SDK
echo "Checking prerequisites..."
if command -v dotnet &> /dev/null; then
    print_status ".NET SDK installed: $(dotnet --version)"
else
    print_error ".NET SDK not found! Please install .NET 8.0 SDK"
    exit 1
fi

# Check MongoDB
echo ""
echo "Checking MongoDB..."
if command -v mongod &> /dev/null; then
    print_status "MongoDB is installed"
    
    # Check if MongoDB is running
    if pgrep -x "mongod" > /dev/null; then
        print_status "MongoDB is running"
    else
        print_warning "MongoDB is not running. Starting MongoDB..."
        sudo systemctl start mongod
        if [ $? -eq 0 ]; then
            print_status "MongoDB started successfully"
        else
            print_error "Failed to start MongoDB"
            exit 1
        fi
    fi
else
    print_warning "MongoDB not found. Would you like to install it? (y/n)"
    read -r response
    if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
        print_status "Installing MongoDB..."
        sudo ./install-mongodb.sh
    else
        print_error "MongoDB is required. Please install it manually or use MongoDB Atlas."
        exit 1
    fi
fi

# Build the project
echo ""
echo "Building the project..."
dotnet build > /dev/null 2>&1
if [ $? -eq 0 ]; then
    print_status "Project built successfully"
else
    print_error "Build failed. Please check the error messages."
    exit 1
fi

# Ask about database initialization
echo ""
print_warning "Would you like to initialize the database with sample data? (y/n)"
print_warning "This will create:"
echo "  - Admin user (username: admin, password: admin123)"
echo "  - Client user (username: client1, password: client123)"
echo "  - Sample categories"
echo "  - Sample books"
read -r response

if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
    print_status "Initializing database..."
    ./init-database.sh
    if [ $? -eq 0 ]; then
        print_status "Database initialized successfully"
    else
        print_error "Database initialization failed"
    fi
fi

# All done
echo ""
echo "=================================================="
echo -e "${GREEN}  Setup Complete!${NC}"
echo "=================================================="
echo ""
echo "To run the application:"
echo "  cd $(pwd)"
echo "  dotnet run"
echo ""
echo "The application will be available at:"
echo "  HTTPS: https://localhost:5001"
echo "  HTTP:  http://localhost:5000"
echo ""
echo "Default credentials:"
echo "  Admin - username: admin, password: admin123"
echo "  Client - username: client1, password: client123"
echo ""
print_warning "Would you like to start the application now? (y/n)"
read -r response

if [[ "$response" =~ ^([yY][eE][sS]|[yY])$ ]]; then
    echo ""
    print_status "Starting the application..."
    echo "Press Ctrl+C to stop the application"
    echo ""
    dotnet run
fi
