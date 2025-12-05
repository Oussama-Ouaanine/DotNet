#!/bin/bash

echo "==============================================="
echo "MongoDB Installation Script for Ubuntu/Debian"
echo "==============================================="

# Check if MongoDB is already installed
if command -v mongod &> /dev/null; then
    echo "MongoDB is already installed!"
    mongod --version
    exit 0
fi

echo "Installing MongoDB..."

# Import the public key
wget -qO - https://www.mongodb.org/static/pgp/server-7.0.asc | sudo apt-key add -

# Create list file for Ubuntu 22.04
echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu jammy/mongodb-org/7.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-7.0.list

# Update package database
sudo apt-get update

# Install MongoDB
sudo apt-get install -y mongodb-org

# Start MongoDB
sudo systemctl start mongod

# Enable MongoDB to start on boot
sudo systemctl enable mongod

# Check status
sudo systemctl status mongod

echo ""
echo "MongoDB installation complete!"
echo "MongoDB is running on: mongodb://localhost:27017"
echo ""
echo "To access MongoDB shell, run: mongosh"
echo "To stop MongoDB: sudo systemctl stop mongod"
echo "To restart MongoDB: sudo systemctl restart mongod"
