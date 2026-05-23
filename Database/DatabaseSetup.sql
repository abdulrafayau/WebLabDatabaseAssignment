-- SQL Database Setup Script for Web Lab Database Assignment
-- This script creates the database and tables for all four applications

-- Create Database
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'WebLabDB')
BEGIN
    CREATE DATABASE [WebLabDB];
END

GO

USE [WebLabDB];

GO

-- Drop existing tables if they exist (for fresh setup)
IF OBJECT_ID(N'dbo.TodoItems', N'U') IS NOT NULL DROP TABLE dbo.TodoItems;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID(N'dbo.Students', N'U') IS NOT NULL DROP TABLE dbo.Students;
IF OBJECT_ID(N'dbo.Contacts', N'U') IS NOT NULL DROP TABLE dbo.Contacts;
IF OBJECT_ID(N'dbo.Products', N'U') IS NOT NULL DROP TABLE dbo.Products;

GO

-- Students Table
CREATE TABLE dbo.Students (
    Id INT PRIMARY KEY IDENTITY(1,1),
    RollNumber NVARCHAR(50) NOT NULL UNIQUE,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    DateOfBirth DATETIME2,
    Department NVARCHAR(100),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);

-- Contacts Table
CREATE TABLE dbo.Contacts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(500),
    City NVARCHAR(100),
    Country NVARCHAR(100),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);

-- Users Table (for To-Do List)
CREATE TABLE dbo.Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(100) NOT NULL UNIQUE,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);

-- TodoItems Table
CREATE TABLE dbo.TodoItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX),
    IsCompleted BIT NOT NULL DEFAULT 0,
    DueDate DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    CONSTRAINT FK_TodoItems_Users FOREIGN KEY (UserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE
);

-- Products Table (Inventory Manager)
CREATE TABLE dbo.Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(255) NOT NULL,
    Sku NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX),
    Price DECIMAL(10,2) NOT NULL,
    QuantityInStock INT NOT NULL DEFAULT 0,
    Category NVARCHAR(100),
    Supplier NVARCHAR(255),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2
);

GO

-- Create Indexes for better performance
CREATE INDEX IX_Students_Email ON dbo.Students(Email);
CREATE INDEX IX_Contacts_Email ON dbo.Contacts(Email);
CREATE INDEX IX_Users_Username ON dbo.Users(Username);
CREATE INDEX IX_Users_Email ON dbo.Users(Email);
CREATE INDEX IX_TodoItems_UserId ON dbo.TodoItems(UserId);
CREATE INDEX IX_Products_Sku ON dbo.Products(Sku);

GO

-- Sample Data for Testing
-- Students
INSERT INTO dbo.Students (RollNumber, FirstName, LastName, Email, PhoneNumber, DateOfBirth, Department)
VALUES 
    ('2024001', 'Ahmed', 'Hassan', 'ahmed.hassan@email.com', '03001234567', '2004-05-15', 'Computer Science'),
    ('2024002', 'Fatima', 'Khan', 'fatima.khan@email.com', '03001234568', '2004-06-20', 'Engineering'),
    ('2024003', 'Ali', 'Ahmed', 'ali.ahmed@email.com', '03001234569', '2004-07-10', 'Business');

-- Contacts
INSERT INTO dbo.Contacts (FirstName, LastName, Email, PhoneNumber, Address, City, Country)
VALUES 
    ('Muhammad', 'Rizvi', 'rizvi@email.com', '03101234567', '123 Main St', 'Karachi', 'Pakistan'),
    ('Ayesha', 'Malik', 'ayesha@email.com', '03201234567', '456 Oak Ave', 'Lahore', 'Pakistan'),
    ('Hassan', 'Ali', 'hassan.ali@email.com', '03301234567', '789 Pine Rd', 'Islamabad', 'Pakistan');

-- Products
INSERT INTO dbo.Products (ProductName, Sku, Description, Price, QuantityInStock, Category, Supplier)
VALUES 
    ('Laptop Dell XPS 13', 'DEL-XPS-001', 'High-performance laptop with Intel Core i7', 1299.99, 15, 'Electronics', 'Dell Inc.'),
    ('Wireless Mouse', 'LOG-MOUSE-001', 'Bluetooth wireless mouse', 29.99, 50, 'Electronics', 'Logitech'),
    ('USB-C Hub', 'ANK-HUB-001', '7-in-1 USB-C Hub with HDMI', 49.99, 25, 'Electronics', 'Anker'),
    ('Programming Book', 'BK-PROG-001', 'Clean Code: A Handbook of Agile Software Craftsmanship', 39.99, 20, 'Books', 'Prentice Hall'),
    ('Desk Lamp LED', 'PHI-LAMP-001', 'Adjustable LED desk lamp with USB charging', 24.99, 30, 'Home & Garden', 'Philips');

GO

PRINT 'Database setup completed successfully!';
