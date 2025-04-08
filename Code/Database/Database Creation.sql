CREATE DATABASE ReMarket;
USE ReMarket;

CREATE TABLE Account (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(100) NOT NULL,
    photoId INT,
    FOREIGN KEY (photoId) REFERENCES Photo(id)
);

CREATE TABLE `Order` (
    id INT AUTO_INCREMENT PRIMARY KEY,
    shipTo VARCHAR(255) NOT NULL,
    shipped DATE,
    description TEXT,
    accountId INT,
    paymentId INT,
    FOREIGN KEY (accountId) REFERENCES Account(id),
    FOREIGN KEY (paymentId) REFERENCES Payment(id)
);

CREATE TABLE Payment (
    id INT AUTO_INCREMENT PRIMARY KEY,
    paidOn DATE,
    total DECIMAL(10, 2) NOT NULL,
    accountId INT,
    FOREIGN KEY (accountId) REFERENCES Account(id)
);

CREATE TABLE Review (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    score INT NOT NULL,
    description TEXT,
    accountId INT,
    listingId INT,
    FOREIGN KEY (accountId) REFERENCES Account(id),
    FOREIGN KEY (listingId) REFERENCES Listing(id)
);

CREATE TABLE Listing (
    id INT AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    status VARCHAR(50) NOT NULL
);

CREATE TABLE Category (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE Description (
    id INT AUTO_INCREMENT PRIMARY KEY,
    header VARCHAR(255) NOT NULL,
    paragraph TEXT
);

CREATE TABLE Photo (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    bytes LONGBLOB NOT NULL
);

CREATE TABLE Wishlist (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL
);

CREATE TABLE WebUser (
    id INT AUTO_INCREMENT PRIMARY KEY,
    logInId VARCHAR(100) NOT NULL
);

-- Many-to-many relationship between Order and Listing
CREATE TABLE OrderListing (
    orderId INT,
    listingId INT,
    PRIMARY KEY (orderId, listingId),
    FOREIGN KEY (orderId) REFERENCES `Order`(id),
    FOREIGN KEY (listingId) REFERENCES Listing(id)
);

-- Many-to-one relationship between Listing and Photo
CREATE TABLE ListingPhoto (
    listingId INT,
    photoId INT,
    PRIMARY KEY (listingId, photoId),
    FOREIGN KEY (listingId) REFERENCES Listing(id),
    FOREIGN KEY (photoId) REFERENCES Photo(id)
);
