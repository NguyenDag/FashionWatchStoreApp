--create database ProjectPRN
USE [master]
GO

/*******************************************************************************
   Drop database if it exists
********************************************************************************/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'ProjectPRN')
BEGIN
	ALTER DATABASE ProjectPRN SET OFFLINE WITH ROLLBACK IMMEDIATE;
	ALTER DATABASE ProjectPRN SET ONLINE;
	DROP DATABASE ProjectPRN;
END

GO

CREATE DATABASE ProjectPRN
GO

USE ProjectPRN
GO

/*******************************************************************************
	Drop tables if exists
*******************************************************************************/
DECLARE @sql nvarchar(MAX) 
SET @sql = N'' 

SELECT @sql = @sql + N'ALTER TABLE ' + QUOTENAME(KCU1.TABLE_SCHEMA) 
    + N'.' + QUOTENAME(KCU1.TABLE_NAME) 
    + N' DROP CONSTRAINT ' -- + QUOTENAME(rc.CONSTRAINT_SCHEMA)  + N'.'  -- not in MS-SQL
    + QUOTENAME(rc.CONSTRAINT_NAME) + N'; ' + CHAR(13) + CHAR(10) 
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 

INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 
    ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
    AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
    AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME 

EXECUTE(@sql) 

GO
DECLARE @sql2 NVARCHAR(max)=''

SELECT @sql2 += ' Drop table ' + QUOTENAME(TABLE_SCHEMA) + '.'+ QUOTENAME(TABLE_NAME) + '; '
FROM   INFORMATION_SCHEMA.TABLES
WHERE  TABLE_TYPE = 'BASE TABLE'

Exec Sp_executesql @sql2 
GO



CREATE TABLE Brand (
  brandID INT PRIMARY KEY IDENTITY(1,1),--id
  brandName NVARCHAR(255) NOT NULL,--ten nha cung cap
  brandAddress NVARCHAR(255),--dia chi nha cung cap
  brandPhone NVARCHAR(20),--so dthoai nha cc
  brandEmail NVARCHAR(255),-- dia chi mail
  brandStatus BIT DEFAULT 1
);

--Bronze, Silver, Gold, Diamond
CREATE TABLE AccountRank (
  accountRankID INT PRIMARY KEY IDENTITY(1,1),
  accountRankName NVARCHAR(100),
  minAmount DECIMAL(18,2)
);

-- Create Accounts table
CREATE TABLE Account (
  accountID INT PRIMARY KEY IDENTITY(1,1),
  username NVARCHAR(255) NOT NULL,
  [password] NVARCHAR(255) NOT NULL,
  [role] INT NOT NULL, -- 0 for admin, 1 for user
  fullname NVARCHAR(255) NOT NULL,--ten
  [address] NVARCHAR(255),--dia chi
  phone NVARCHAR(20),--sdt
  email NVARCHAR(255),--mail
  dob DATE,
  avatar NVARCHAR(255),
  gender BIT, --0 la nam, 1 la nu
  [status] BIT DEFAULT 1,
  accountRankID INT,
  FOREIGN KEY (accountRankID) REFERENCES AccountRank(accountRankID)
);

-- Create Categories table
CREATE TABLE Category (
  categoryID INT PRIMARY KEY IDENTITY(1,1),
  categoryName NVARCHAR(255) NOT NULL,
  categoryDescription NVARCHAR(MAX)
);

-- Create Products table
CREATE TABLE Product (
  productID INT PRIMARY KEY IDENTITY(1,1),
  productName NVARCHAR(255) NOT NULL,
  categoryID INT NOT NULL,
  brandID INT NOT NULL,
  productDescription NVARCHAR(MAX),
  quantity INT NOT NULL ,--số lượng sản phẩm
  priceBuy DECIMAL(18,2) NOT NULL,
  priceSell DECIMAL(18,2) NOT NULL, --gia san phẩm
  [status] INT DEFAULT 1,
  FOREIGN KEY (categoryID) REFERENCES Category(categoryID),
  FOREIGN KEY (brandID) REFERENCES Brand(brandID)
);

CREATE TABLE Cart (
  cartID INT PRIMARY KEY IDENTITY(1,1),   -- Khóa chính cho giỏ hàng
  accountID INT NOT NULL,                               -- ID tài khoản người dùng
  productID INT NOT NULL,                               -- ID sản phẩm
  quantity INT NOT NULL,                  -- Số lượng sản phẩm trong giỏ
  FOREIGN KEY (accountID) REFERENCES Account(accountID),  -- Khóa ngoại liên kết tới bảng Accounts
  FOREIGN KEY (productID) REFERENCES Product(productID)   -- Khóa ngoại liên kết tới bảng Products
);


-- Create ShippingInformation table
CREATE TABLE Shipping (
  shipID INT PRIMARY KEY IDENTITY(1,1),
  shipMethod NVARCHAR(255),
  shipCost DECIMAL(18, 2),
  shipTime INT
);

CREATE TABLE Discount(
    discountID INT PRIMARY KEY IDENTITY(1,1),  -- Tự động tăng
    discountName NVARCHAR(100) NOT NULL,
    [percent] FLOAT NOT NULL,
	accountRankID int,
    minCost DECIMAL(18,2),
	endDate DATE,
    [status] bit
);

-- Create Orders table
CREATE TABLE [Order] (
  orderID INT PRIMARY KEY IDENTITY(1,1),
  accountID INT,
  orderDate DATETIME DEFAULT GETDATE(),
  totalCost DECIMAL(18, 2),
  actualCost DECIMAL(18, 2),
  [status] NVARCHAR(50), --Pending, Shipping, Canceled, Completed
  paymentType INT, -- 1-Online; 2-COD
  shipID INT,
  shipDate DATETIME,
  receiveDate DATETIME,
  receiveName NVARCHAR(100),
  receivePhone NVARCHAR(20),
  receiveAddress NVARCHAR(MAX),
  discountID INT,
  note NVARCHAR(MAX),
  FOREIGN KEY (accountID) REFERENCES Account(accountID),
  FOREIGN KEY (shipID) REFERENCES Shipping(shipID),
  FOREIGN KEY (discountID) REFERENCES Discount(discountID)
);

-- Create OrderDetails table
CREATE TABLE OrderDetails (
  orderDetailID INT PRIMARY KEY IDENTITY(1,1),
  orderID INT,
  productID INT,
  quantity INT,
  price INT,
  FOREIGN KEY (orderID) REFERENCES [Order](orderID),
  FOREIGN KEY (productID) REFERENCES Product(productID)
);

-- Create ProductImages table
CREATE TABLE ProductImages (
  imageID INT PRIMARY KEY IDENTITY(1,1),
  productID INT,
  imageURL NVARCHAR(MAX),
  [isMain] BIT,
  [status] BIT DEFAULT 1,
  FOREIGN KEY (productID) REFERENCES Product(productID)
);

-- Create ProductReviews table
CREATE TABLE Review (
  reviewID INT PRIMARY KEY IDENTITY(1,1),
  productID INT,
  accountID INT,
  reviewText NVARCHAR(MAX),
  reviewRating INT,
  reviewDate DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (productID) REFERENCES Product(productID),
  FOREIGN KEY (accountID) REFERENCES Account(accountID)
);

-- Create WarrantyInformation table bao hanh
--CREATE TABLE Warranty (
--  warrantyID INT PRIMARY KEY IDENTITY(1,1),
--  productID INT,
--  warPeriod INT,
--  warDescription TEXT,
--  FOREIGN KEY (productID) REFERENCES Product(productID)
--);

-- chèn DATA
-- Chèn dữ liệu mẫu vào bảng Brand
INSERT INTO Brand (brandName, brandAddress, brandPhone, brandEmail) VALUES
('Apple', 'California, USA', '1800123456', 'support@apple.com'),
('Samsung', 'Seoul, South Korea', '1800654321', 'support@samsung.com'),
('Rolex', 'Geneva, Switzerland', '1800765432', 'support@rolex.com'),
('Casio', 'Tokyo, Japan', '1800543210', 'support@casio.com');

-- Chèn dữ liệu mẫu vào bảng AccountRank
INSERT INTO AccountRank (accountRankName, minAmount) VALUES
('Bronze', 0),
('Silver', 500),
('Gold', 1000),
('Diamond', 5000);

-- Chèn dữ liệu mẫu vào bảng Account
INSERT INTO Account (username, password, role, fullname, address, phone, email, dob, gender, accountRankID) VALUES
('admin', 'admin123', 0, 'Administrator', '123 Admin St', '0123456789', 'admin@example.com', '1990-01-01', 0, NULL),
('user1', 'user123', 1, 'John Doe', '456 User Rd', '0987654321', 'johndoe@example.com', '1995-05-15', 0, 1);

-- Chèn dữ liệu mẫu vào bảng Category
INSERT INTO Category (categoryName, categoryDescription) VALUES
('Smartphones', 'Latest smartphones from top brands'),
('Laptops', 'High-performance laptops for all needs'),
('Watches', 'Luxury and smart watches from top brands');

-- Chèn dữ liệu mẫu vào bảng Product
INSERT INTO Product (productName, categoryID, brandID, productDescription, quantity, priceBuy, priceSell) VALUES
('iPhone 15', 1, 1, 'Apple iPhone 15 with A16 Bionic chip', 50, 900, 999),
('Galaxy S23', 1, 2, 'Samsung Galaxy S23 with Snapdragon 8 Gen 2', 60, 850, 899),
('Rolex Submariner', 3, 3, 'Luxury Rolex watch with stainless steel body', 10, 8000, 10000),
('Casio G-Shock', 3, 4, 'Durable and stylish Casio G-Shock watch', 30, 100, 150);

-- Chèn dữ liệu mẫu vào bảng Cart
INSERT INTO Cart (accountID, productID, quantity) VALUES
(2, 1, 2),
(2, 2, 1),
(2, 3, 1);

-- Chèn dữ liệu mẫu vào bảng Shipping
INSERT INTO Shipping (shipMethod, shipCost, shipTime) VALUES
('Standard Shipping', 5.99, 5),
('Express Shipping', 15.99, 2);

-- Chèn dữ liệu mẫu vào bảng Discount
INSERT INTO Discount (discountName, [percent], accountRankID, minCost, endDate, status) VALUES
('New Year Sale', 10, 2, 200, '2025-12-31', 1);

-- Chèn dữ liệu mẫu vào bảng Order
INSERT INTO [Order] (accountID, totalCost, actualCost, status, paymentType, shipID, receiveName, receivePhone, receiveAddress, discountID) VALUES
(2, 1898, 1708.2, 'Pending', 1, 1, 'John Doe', '0987654321', '456 User Rd', 1),
(2, 10000, 9000, 'Pending', 2, 2, 'John Doe', '0987654321', '456 User Rd', NULL);

-- Chèn dữ liệu mẫu vào bảng OrderDetails
INSERT INTO OrderDetails (orderID, productID, quantity, price) VALUES
(1, 1, 2, 999),
(1, 2, 1, 899),
(2, 3, 1, 10000);

-- Chèn dữ liệu mẫu vào bảng ProductImages
INSERT INTO ProductImages (productID, imageURL, isMain) VALUES
(1, 'iphone15.jpg', 1),
(2, 'galaxys23.jpg', 1),
(3, 'rolex_submariner.jpg', 1),
(4, 'casio_gshock.jpg', 1);

-- Chèn dữ liệu mẫu vào bảng Review
INSERT INTO Review (productID, accountID, reviewText, reviewRating) VALUES
(1, 2, 'Great phone with amazing performance!', 5),
(2, 2, 'Solid build and smooth performance.', 4),
(3, 2, 'Luxury watch, excellent quality!', 5),
(4, 2, 'Very durable and stylish watch.', 4);
