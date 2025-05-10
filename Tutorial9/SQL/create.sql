-- Created by Vertabelo (http://vertabelo.com)
-- Last modification date: 2021-04-05 12:56:53.13

-- tables
-- Table: Order
CREATE TABLE "Order" (
    IdOrder int  NOT NULL IDENTITY,
    IdProduct int  NOT NULL,
    Amount int  NOT NULL,
    CreatedAt datetime  NOT NULL,
    FulfilledAt datetime  NULL,
    CONSTRAINT Order_pk PRIMARY KEY  (IdOrder)
);

-- Table: Product
CREATE TABLE Product (
    IdProduct int  NOT NULL IDENTITY,
    Name nvarchar(200)  NOT NULL,
    Description nvarchar(200)  NOT NULL,
    Price numeric(25,2)  NOT NULL,
    CONSTRAINT Product_pk PRIMARY KEY  (IdProduct)
);

-- Table: Product_Warehouse
CREATE TABLE Product_Warehouse (
    IdProductWarehouse int  NOT NULL IDENTITY,
    IdWarehouse int  NOT NULL,
    IdProduct int  NOT NULL,
    IdOrder int  NOT NULL,
    Amount int  NOT NULL,
    Price numeric(25,2)  NOT NULL,
    CreatedAt datetime  NOT NULL,
    CONSTRAINT Product_Warehouse_pk PRIMARY KEY  (IdProductWarehouse)
);

-- Table: Warehouse
CREATE TABLE Warehouse (
    IdWarehouse int  NOT NULL IDENTITY,
    Name nvarchar(200)  NOT NULL,
    Address nvarchar(200)  NOT NULL,
    CONSTRAINT Warehouse_pk PRIMARY KEY  (IdWarehouse)
);

-- foreign keys
-- Reference: Product_Warehouse_Order (table: Product_Warehouse)
ALTER TABLE Product_Warehouse ADD CONSTRAINT Product_Warehouse_Order
    FOREIGN KEY (IdOrder)
    REFERENCES "Order" (IdOrder);

-- Reference: Receipt_Product (table: Order)
ALTER TABLE "Order" ADD CONSTRAINT Receipt_Product
    FOREIGN KEY (IdProduct)
    REFERENCES Product (IdProduct);

-- Reference: _Product (table: Product_Warehouse)
ALTER TABLE Product_Warehouse ADD CONSTRAINT _Product
    FOREIGN KEY (IdProduct)
    REFERENCES Product (IdProduct);

-- Reference: _Warehouse (table: Product_Warehouse)
ALTER TABLE Product_Warehouse ADD CONSTRAINT _Warehouse
    FOREIGN KEY (IdWarehouse)
    REFERENCES Warehouse (IdWarehouse);

-- End of file.

GO

INSERT INTO Warehouse(Name, Address) VALUES
('Warsaw', 'Kwiatowa 12'),
('Krakow', 'Zielona 5'),
('Gdansk', 'Morska 44'),
('Wroclaw', 'Szafirowa 10'),
('Lodz', 'Lesna 22');

GO

INSERT INTO Product(Name, Description, Price) VALUES
('Abacavir', 'Used for HIV treatment', 25.50),
('Acyclovir', 'Used for herpes treatment', 45.00),
('Allopurinol', 'Used for gout treatment', 30.80),
('Ibuprofen', 'Pain reliever', 15.00),
('Paracetamol', 'Fever reducer', 12.75);

GO

INSERT INTO "Order"(IdProduct, Amount, CreatedAt) VALUES
(1, 125, GETDATE()),
(2, 200, GETDATE()),
(3, 50, GETDATE()),
(4, 300, GETDATE()),
(5, 100, GETDATE());

GO

INSERT INTO Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) VALUES
(1, 1, 1, 125, 3187.50, GETDATE()),
(2, 2, 2, 200, 9000.00, GETDATE()),
(3, 3, 3, 50, 1540.00, GETDATE()),
(4, 4, 4, 300, 4500.00, GETDATE()),
(5, 5, 5, 100, 1275.00, GETDATE());