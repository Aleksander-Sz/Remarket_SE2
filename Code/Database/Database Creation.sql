-- Create the database
CREATE DATABASE EcommercePlatform;

-- Use the created database
USE EcommercePlatform;

-- Create the 'User' table
CREATE TABLE User (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Surname VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL
);

-- Create the 'Seller' table
CREATE TABLE Seller (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Surname VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL
);

-- Create the 'Product' table
CREATE TABLE Product (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Category VARCHAR(100) NOT NULL,
    SellerId INT,
    Date DATE NOT NULL,
    Available BOOLEAN NOT NULL,
    FOREIGN KEY (SellerId) REFERENCES Seller(Id)
);

-- Create the 'Sale' table
CREATE TABLE Sale (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Date DATE NOT NULL,
    ProductId INT,
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);
