-- Create the database
CREATE DATABASE EcommercePlatform;

-- Use the created database
USE EcommercePlatform;

-- Create the 'User' table
CREATE TABLE Account (
    id INT AUTO_INCREMENT PRIMARY KEY,
    userame VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL
);
-- Create the 'Web User' table
CREATE TABLE WebUser (
    loginId VARCHAR(100) UNIQUE NOT NULL,
);

-- Create the 'Reviev' table
CREATE TABLE Reviev (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    score VARCHAR(100) NOT NULL,
    descriprion VARCHAR(100) UNIQUE NOT NULL,
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
