CREATE PROCEDURE AddProductToWarehouse @IdProduct INT, @IdWarehouse INT, @Amount INT,  
@CreatedAt DATETIME
AS  
BEGIN  
    DECLARE @IdProductFromDb INT, @IdOrder INT, @Price DECIMAL(5,2);  
    
    SELECT TOP 1 @IdOrder = o.IdOrder
    FROM "Order" o   
    WHERE IdProduct = @IdProduct;
    
    SELECT @IdProductFromDb=Product.IdProduct, @Price=Product.Price FROM Product WHERE IdProduct=@IdProduct  
    
    IF @IdProductFromDb IS NULL  
    BEGIN  
    RAISERROR('Invalid parameter: Provided IdProduct does not exist', 16, 0);  
    END;  
    
    IF @IdOrder IS NULL  
    BEGIN  
    RAISERROR('Invalid parameter: There is no order to fulfill', 16, 0);  
    END;  
    
    IF NOT EXISTS(SELECT 1 FROM Warehouse WHERE IdWarehouse=@IdWarehouse)  
    BEGIN  
    RAISERROR('Invalid parameter: Provided IdWarehouse does not exist', 16, 0);  
    END;  
    
    IF (SELECT 1 FROM Product_Warehouse WHERE IdOrder = @IdOrder) IS NOT NULL
    BEGIN 
        RAISERROR('Order has already been completed', 16, 0);
    END;
    
    SET XACT_ABORT ON;  
    BEGIN TRAN;  
    
    UPDATE "Order" SET  
    FulfilledAt=@CreatedAt  
    WHERE IdOrder=@IdOrder;  
    
    INSERT INTO Product_Warehouse(IdWarehouse,   
    IdProduct, IdOrder, Amount, Price, CreatedAt)  
    VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Amount*@Price, @CreatedAt);  
    
    SELECT @@IDENTITY AS NewId;
    
    COMMIT;  
END