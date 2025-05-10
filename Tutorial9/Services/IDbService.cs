using Tutorial9.Model.DTOs;

namespace Tutorial9.Services;

public interface IDbService
{
    Task<int> PutProduct(WarehouseDto dto);
    Task ProcedureAsync();
    Task<int> GetProductPrice(int idProduct);
    Task<int> GetOrderId(WarehouseDto dto);
    Task<int> OrderCompleted(WarehouseDto dto);
}