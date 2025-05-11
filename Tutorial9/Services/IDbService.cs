using Tutorial9.Model.DTOs;

namespace Tutorial9.Services;

public interface IDbService
{
    Task<int> PutProduct(WarehouseDto dto);
    Task<int> ProcedureAsync(WarehouseDto dto);
    Task<int> GetProductPrice(int idProduct);
    Task<int> GetOrderId(WarehouseDto dto);
    Task<int> OrderCompleted(WarehouseDto dto);
    Task<bool> DoesWarehouseExist(int idWarehouse);
}