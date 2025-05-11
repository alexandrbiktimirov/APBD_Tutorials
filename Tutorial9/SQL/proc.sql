CREATE OR ALTER PROCEDURE AddProductToWarehouse @IdProduct INT, @IdWarehouse INT, @Amount INT,  
@CreatedAt DATETIME
AS  
BEGIN  
    DECLARE @IdProductFromDb INT, @IdOrder INT, @Price DECIMAL(5,2);

    SELECT TOP 1 @IdOrder = IdOrder
    FROM "Order"
    WHERE
        IdProduct = @IdProduct AND
        Amount = @Amount AND
        CreatedAt < @CreatedAt
    ORDER BY CreatedAt;

    SELECT @IdProductFromDb=Product.IdProduct, @Price=Product.Price FROM Product WHERE IdProduct=@IdProduct  
    
    IF @IdProductFromDb IS NULL  
    BEGIN  
        RAISERROR('Invalid parameter: Provided IdProduct does not exist', 16, 0);
        RETURN;
    END;  
    
    IF @IdOrder IS NULL  
    BEGIN  
        RAISERROR('Invalid parameter: There is no order to fulfill', 16, 0);
        RETURN;
    END;  
    
    IF @Amount <= 0
    BEGIN 
        RAISERROR('Invalid parameter: Amount has to be greater than 0', 16, 0);
        RETURN;
    END;
    
    IF NOT EXISTS (SELECT 1 FROM Warehouse WHERE IdWarehouse=@IdWarehouse)  
    BEGIN  
        RAISERROR('Invalid parameter: Provided IdWarehouse does not exist', 16, 0);
        RETURN;
    END;  
    
    IF EXISTS (SELECT 1 FROM Product_Warehouse WHERE IdOrder = @IdOrder)
    BEGIN 
        RAISERROR('Order has already been completed', 16, 0);
        RETURN;    
    END;
    
    SET XACT_ABORT ON;  
    BEGIN TRAN;  
    
    UPDATE "Order" SET  
    FulfilledAt=GETDATE()  
    WHERE IdOrder=@IdOrder;  
    
    INSERT INTO Product_Warehouse(IdWarehouse,   
    IdProduct, IdOrder, Amount, Price, CreatedAt)  
    VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Amount*@Price, GETDATE());  
    
    SELECT SCOPE_IDENTITY() AS NewId;
    
    COMMIT;  
END