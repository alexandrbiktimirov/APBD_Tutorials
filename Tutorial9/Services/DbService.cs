using System.Data;
using System.Data.Common;
using System.Transactions;
using Microsoft.Data.SqlClient;
using Tutorial9.Exceptions;
using Tutorial9.Model.DTOs;

namespace Tutorial9.Services;

public class DbService : IDbService
{
    private readonly IConfiguration _configuration;

    public DbService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> GetProductPrice(int idProduct)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand("SELECT Price FROM Product WHERE IdProduct = @id", connection);
        await connection.OpenAsync();
        
        command.Parameters.AddWithValue("@id", idProduct);
        var price = Convert.ToInt32(await command.ExecuteScalarAsync() ?? throw new ProductDoesNotExistException("Product does not exist"));
        
        return price;
    }

    public async Task<int> GetOrderId(WarehouseDto dto)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command =
            new SqlCommand(
                "SELECT IdOrder FROM [Order] WHERE IdProduct = @id AND Amount = @amount AND CreatedAt < @createdAt",
                connection);
        await connection.OpenAsync();
        
        command.Parameters.AddWithValue("@id", dto.IdProduct);
        command.Parameters.AddWithValue("@amount", dto.Amount);
        command.Parameters.AddWithValue("@createdAt", dto.CreatedAt);

        var result = Convert.ToInt32(await command.ExecuteScalarAsync() ?? throw new OrderDoesNotExistException("Order does not exist"));
        
        return result;
    }

    public async Task<int> OrderCompleted(WarehouseDto dto)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand completedOrderCommand = new SqlCommand("SELECT 1 FROM Product_Warehouse WHERE IdOrder = @id", connection);
        await connection.OpenAsync();
        
        var orderId = await GetOrderId(dto);
        completedOrderCommand.Parameters.AddWithValue("@id", orderId);

        return Convert.ToInt32(await completedOrderCommand.ExecuteScalarAsync());
    }

    public async Task<int> PutProduct(WarehouseDto dto)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        var productPrice = await GetProductPrice(dto.IdProduct);
        var idOrder = await GetOrderId(dto);
        var orderHasBeenCompleted = await OrderCompleted(dto);
        if (orderHasBeenCompleted > 0)
        {
            throw new OrderHasBeenCompletedException("Order has already been completed");
        }
        
        command.Connection = connection;
        await connection.OpenAsync();

        DbTransaction transaction = await connection.BeginTransactionAsync();
        command.Transaction = transaction as SqlTransaction;
        
        try
        {
            command.CommandText = "UPDATE [Order] SET FulfilledAt = @time WHERE IdOrder = @idOrder;";
            command.Parameters.AddWithValue("@time", DateTime.Now);
            command.Parameters.AddWithValue("@idOder", idOrder);
            
            await command.ExecuteNonQueryAsync();
            
            command.Parameters.Clear();
            
            command.CommandText = "INSERT INTO Product_Warehouse VALUES (@idWarehouse, @idProduct, @idOrder, @amount, @price, @createdAt) SELECT SCOPE_IDENTITY();";
            command.Parameters.AddWithValue("@idWarehouse", dto.IdWarehouse);
            command.Parameters.AddWithValue("@idProduct", dto.IdProduct);
            command.Parameters.AddWithValue("@idOrder", idOrder);
            command.Parameters.AddWithValue("@amount", dto.Amount);
            command.Parameters.AddWithValue("@price", productPrice * dto.Amount);
            command.Parameters.AddWithValue("@createdAt", DateTime.Now);
            
            var id = Convert.ToInt32(await command.ExecuteScalarAsync() ?? throw new TransactionException("Transaction exception occured"));
            await transaction.CommitAsync();

            return id;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new TransactionException("Transaction exception occured");
        }
    }

    public async Task<int> ProcedureAsync(WarehouseDto dto)
    {
        await using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using SqlCommand command = new SqlCommand();
        
        command.Connection = connection;
        await connection.OpenAsync();
        
        command.CommandText = "AddProductToWarehouse";
        command.CommandType = CommandType.StoredProcedure;
        
        command.Parameters.AddWithValue("@IdProduct", dto.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", dto.IdWarehouse);
        command.Parameters.AddWithValue("@Amount", dto.Amount);
        command.Parameters.AddWithValue("@CreatedAt", dto.CreatedAt);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }
}