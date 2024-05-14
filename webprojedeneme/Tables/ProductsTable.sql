CREATE TABLE Products (
    ProductId INT PRIMARY KEY,
    CategoryId INT,
    ProductName NVARCHAR(100),
    UnitsInStock SMALLINT,
    UnitPrice DECIMAL(18,2)
);
