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

-- Create the 'Wishlist' table
CREATE TABLE Wishlist (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255)
);

-- Create the 'Order' table
CREATE TABLE Order (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ProductId INT,
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
    description VARCHAR(255)
);
-- Create the 'Payment' table
CREATE TABLE Payment (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(255)
);