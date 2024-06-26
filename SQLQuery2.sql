-- Tạo cơ sở dữ liệu với charset UTF-8 và collation utf8_unicode_ci để hỗ trợ tiếng Việt
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'OnlineStore')
BEGIN
    CREATE DATABASE OnlineStore;
END
GO

-- Sử dụng cơ sở dữ liệu vừa tạo
USE OnlineStore;
GO

-- Tạo bảng lưu thông tin khách hàng
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
BEGIN
    CREATE TABLE Customer (
        CustomerID INT IDENTITY(1,1) PRIMARY KEY,
        FirstName NVARCHAR(50),
        LastName NVARCHAR(50),
        Email NVARCHAR(100) UNIQUE,
        Phone NVARCHAR(15),
        Address NVARCHAR(255),
        Password NVARCHAR(255)
    );
END
GO

-- Tạo bảng lưu thông tin danh mục sản phẩm
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category]') AND type in (N'U'))
BEGIN
    CREATE TABLE Category (
        CategoryID INT IDENTITY(1,1) PRIMARY KEY,
        CategoryName NVARCHAR(100),
        Description NVARCHAR(255)
    );
END
GO

-- Tạo bảng lưu thông tin sản phẩm
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product]') AND type in (N'U'))
BEGIN
    CREATE TABLE Product (
        ProductID INT IDENTITY(1,1) PRIMARY KEY,
        ProductName NVARCHAR(100),
        Description NVARCHAR(255),
        Price DECIMAL(10, 2),
        ImageURL NVARCHAR(255),
        CategoryID INT,
        SaleOff DECIMAL(10, 2),
        Quantity INT,
        FOREIGN KEY (CategoryID) REFERENCES Category(CategoryID)
    );
END
GO

-- Tạo bảng lưu thông tin giỏ hàng
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cart]') AND type in (N'U'))
BEGIN
    CREATE TABLE Cart (
        CartID INT IDENTITY(1,1) PRIMARY KEY,
        CustomerID INT,
        CreatedDate DATETIME DEFAULT GETDATE(),
        Total DECIMAL(10, 2),
        FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
    );
END
GO

-- Tạo bảng lưu chi tiết giỏ hàng
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CartDetail]') AND type in (N'U'))
BEGIN
    CREATE TABLE CartDetail (
        CartDetailID INT IDENTITY(1,1) PRIMARY KEY,
        CartID INT,
        ProductID INT,
        Quantity INT,
        FOREIGN KEY (CartID) REFERENCES Cart(CartID),
        FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
    );
END
GO

-- Tạo bảng lưu thông tin hóa đơn
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Invoice]') AND type in (N'U'))
BEGIN
    CREATE TABLE Invoice (
        InvoiceID INT IDENTITY(1,1) PRIMARY KEY,
        CustomerID INT,
        InvoiceDate DATETIME DEFAULT GETDATE(),
        ShipAddress NVARCHAR(255),
        Phone NVARCHAR(15),
        Status NVARCHAR(50),
        Total DECIMAL(10, 2),
        Date DATETIME,
        FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID)
    );
END
GO

-- Tạo bảng lưu chi tiết hóa đơn
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InvoiceDetail]') AND type in (N'U'))
BEGIN
    CREATE TABLE InvoiceDetail (
        InvoiceDetailID INT IDENTITY(1,1) PRIMARY KEY,
        InvoiceID INT,
        ProductID INT,
        Quantity INT,
        UnitPrice DECIMAL(10, 2),
        FOREIGN KEY (InvoiceID) REFERENCES Invoice(InvoiceID),
        FOREIGN KEY (ProductID) REFERENCES Product(ProductID)
    );
END
GO

-- Thêm một số bản ghi mẫu vào bảng Category
INSERT INTO Category (CategoryName, Description)
VALUES
(N'Quần áo', N'Các sản phẩm thời trang như áo quần, váy, đầm, ...'),
(N'Giày dép', N'Các sản phẩm giày dép thời trang'),
(N'Phụ kiện', N'Các loại phụ kiện thời trang như túi xách, mũ, kính mắt, ...');
GO

-- Thêm một số bản ghi mẫu vào bảng Product
INSERT INTO Product (ProductName, Description, Price, ImageURL, CategoryID, SaleOff, Quantity)
VALUES
(N'Áo phông', N'Áo phông cotton mềm mại', 150000, 'http://example.com/images/aophong.jpg', 1, 0, 100),
(N'Quần jeans', N'Quần jeans nam thời trang', 350000, 'http://example.com/images/quanjeans.jpg', 1, 0, 50),
(N'Giày thể thao', N'Giày thể thao nhẹ nhàng, thoải mái', 500000, 'http://example.com/images/giaythethao.jpg', 2, 0, 30),
(N'Váy đầm', N'Váy đầm thiết kế sang trọng', 400000, 'http://example.com/images/vaydam.jpg', 1, 10, 20),
(N'Áo khoác', N'Áo khoác giữ ấm mùa đông', 600000, 'http://example.com/images/aokhoac.jpg', 1, 5, 15),
(N'Sandal nữ', N'Sandal nữ phong cách', 200000, 'http://example.com/images/sandalnu.jpg', 2, 0, 25),
(N'Bốt nam', N'Bốt nam cổ cao', 800000, 'http://example.com/images/botnam.jpg', 2, 15, 10),
(N'Nón lưỡi trai', N'Nón lưỡi trai thời trang', 100000, 'http://example.com/images/nonluoitrai.jpg', 3, 0, 50),
(N'Kính râm', N'Kính râm chống tia UV', 150000, 'http://example.com/images/kinhram.jpg', 3, 0, 30),
(N'Túi xách nữ', N'Túi xách nữ hàng hiệu', 700000, 'http://example.com/images/tuixachnu.jpg', 3, 0, 20),
(N'Đầm dạ hội', N'Đầm dạ hội sang trọng', 900000, 'http://example.com/images/damdahoi.jpg', 1, 20, 5),
(N'Áo sơ mi', N'Áo sơ mi công sở', 250000, 'http://example.com/images/aosomi.jpg', 1, 0, 40),
(N'Quần short', N'Quần short nam mát mẻ', 200000, 'http://example.com/images/quanshort.jpg', 1, 0, 60),
(N'Giày cao gót', N'Giày cao gót thời trang', 500000, 'http://example.com/images/giaycaogot.jpg', 2, 0, 35),
(N'Áo len', N'Áo len giữ ấm', 300000, 'http://example.com/images/aolen.jpg', 1, 0, 25),
(N'Dép lê', N'Dép lê nam nữ', 50000, 'http://example.com/images/deple.jpg', 2, 0, 100),
(N'Ví da', N'Ví da bò thật', 450000, 'http://example.com/images/vida.jpg', 3, 0, 15),
(N'Mũ len', N'Mũ len giữ ấm', 70000, 'http://example.com/images/mulen.jpg', 3, 0, 30),
(N'Tất vớ', N'Tất vớ nam nữ', 20000, 'http://example.com/images/tatvo.jpg', 3, 0, 200),
(N'Giày lười', N'Giày lười thời trang', 400000, 'http://example.com/images/giayluoi.jpg', 2, 0, 20),
(N'Balo', N'Balo laptop', 600000, 'http://example.com/images/balo.jpg', 3, 0, 10),
(N'Đồng hồ', N'Đồng hồ đeo tay thời trang', 1500000, 'http://example.com/images/dongho.jpg', 3, 0, 5);
GO

-- Thêm một số bản ghi mẫu vào bảng Customer
INSERT INTO Customer (FirstName, LastName, Email, Phone, Address, Password)
VALUES
(N'Nguyễn', N'Văn A', 'vana@example.com', '0123456789', N'123 Đường ABC, TP.HCM', 'password123'),
(N'Trần', N'Thị B', 'thib@example.com', '0987654321', N'456 Đường XYZ, Hà Nội', 'password456');
GO

-- Thêm một số bản ghi mẫu vào bảng Cart và CartDetail
INSERT INTO Cart (CustomerID, Total)
VALUES
(1, 800000);
GO

INSERT INTO CartDetail (CartID, ProductID, Quantity)
VALUES
(1, 1, 2),
(1, 2, 1);
GO

-- Thêm một số bản ghi mẫu vào bảng Invoice và InvoiceDetail
INSERT INTO Invoice (CustomerID, ShipAddress, Phone, Status, Total, Date)
VALUES
(1, N'123 Đường ABC, TP.HCM', '0123456789', N'Pending', 800000, GETDATE());
GO

INSERT INTO InvoiceDetail (InvoiceID, ProductID, Quantity, UnitPrice)
VALUES
(1, 1, 2, 150000),
(1, 2, 1, 350000);
GO
