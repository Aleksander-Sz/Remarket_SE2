-- Inserting example data into the 'User' table
INSERT INTO User (Name, Surname, Email, Password)
VALUES
('John', 'Doe', 'john.doe@example.com', 'password123'),
('Jane', 'Smith', 'jane.smith@example.com', 'securepass456');

-- Inserting example data into the 'Seller' table
INSERT INTO Seller (Name, Surname, Email, Password)
VALUES
('Alice', 'Johnson', 'alice.johnson@seller.com', 'sellerpass789'),
('Bob', 'Brown', 'bob.brown@seller.com', 'bobpass321');

-- Inserting example data into the 'Product' table
INSERT INTO Product (Name, Category, SellerId, Date, Available)
VALUES
('Laptop', 'Electronics', 1, '2025-03-27', TRUE),
('Smartphone', 'Electronics', 2, '2025-03-26', TRUE),
('Headphones', 'Accessories', 1, '2025-03-20', FALSE),
('Watch', 'Accessories', 2, '2025-03-15', TRUE);

-- Inserting example data into the 'Sale' table
INSERT INTO Sale (Date, ProductId)
VALUES
('2025-03-27', 1),
('2025-03-26', 2),
('2025-03-15', 4);