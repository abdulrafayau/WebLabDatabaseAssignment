#!/bin/bash

# Web Lab Database Assignment - Automated Setup Script (Linux/Mac)
# This script sets up the entire project locally

echo ""
echo "==================================================="
echo "  Web Lab Database Assignment - Setup Script"
echo "==================================================="
echo ""

# Check if .NET is installed
echo "Checking for .NET 8.0 SDK..."
if ! command -v dotnet &> /dev/null; then
    echo "ERROR: .NET 8.0 SDK not found. Please install it from https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

echo "✓ .NET SDK found"

# Navigate to Backend folder
cd Backend

echo ""
echo "Installing backend dependencies..."
dotnet restore

echo ""
echo "✓ Backend setup completed!"

echo ""
echo "==================================================="
echo "NEXT STEPS:"
echo "==================================================="
echo ""
echo "1. Database Setup:"
echo "   - Open SQL Server Management Studio"
echo "   - Run the script: Database/DatabaseSetup.sql"
echo ""
echo "2. Run Backend API:"
echo "   - In Backend folder, run: dotnet run"
echo "   - API will be available at https://localhost:7001"
echo ""
echo "3. Run Frontend:"
echo "   - Open Frontend/index.html in your browser"
echo "   - OR use: python -m http.server 8000"
echo ""
echo "==================================================="
echo ""
